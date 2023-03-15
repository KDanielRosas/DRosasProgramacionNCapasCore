using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Data;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace PL.Controllers
{
    public class CargaMasivaUsuarioController : Controller
    {
        private IHostingEnvironment _environment;
        private IConfiguration _configuration;

        public CargaMasivaUsuarioController(IHostingEnvironment environment, IConfiguration configuration)
        {
            _environment = environment;
            _configuration = configuration;
        }
        public ActionResult Index(ML.Result result)
        {            
            string display = HttpContext.Session.GetString("Display");
            string file = HttpContext.Session.GetString("File");
            string session = HttpContext.Session.GetString("PathArchivo");
            string correct = HttpContext.Session.GetString("Correct");
            if (session == null)
            {
                if (display == null || display == "none"){ ViewBag.Display = "none"; }
                ViewBag.Correct = false;
                return View(result);
            }
            else
            {
                if (file != null || file == "") { ViewBag.File = file; }
                if (correct == "true"){ ViewBag.Correct = true; }                
                return View(result);
            }         
                
        }

        [HttpPost]
        public ActionResult Index(IFormFile archivoTxt)
        {            
            string connSQL = "Data Source=.;Initial Catalog=DRosasProgramacionNCapas;Persist Security Info=True;User ID=sa;Password=pass@word1; TrustServerCertificate=True;";
            SqlConnection sqlConnection = new SqlConnection(connSQL);

            int counter = 0;
            string line = "";
            
            string fileDelimiter = ",";

            using (StreamReader reader = new StreamReader(archivoTxt.OpenReadStream()))
            {
                sqlConnection.Open();
                while ((line = reader.ReadLine()) != null)
                {                    
                    if (counter > 0)
                    {
                        //string query = "INSERT INTO Usuario VALUES('" + line.Replace(fileDelimiter, "','") + "')";
                        string query = "EXEC UsuarioAdd '" + line.Replace(fileDelimiter, "','") + "'";

                        //ejecutar query
                        SqlCommand cmd = new SqlCommand(query, sqlConnection);
                        cmd.ExecuteNonQuery();
                    }
                    counter++;
                }
            }
            sqlConnection.Close();
            ViewBag.Message = "Exito en la carga";
            return View("Modal");

        }//Index(archivo Txt)

        [HttpPost]
        public ActionResult Excel()
        {
            IFormFile archivo = Request.Form.Files["archivoExcel"];

            if (HttpContext.Session.GetString("PathArchivo") == null)
            {
                if (archivo != null)
                {
                    //Si el archivo trae información
                    if (archivo.Length > 0)
                    {
                        //obtener el nombre de nuestro archivo
                        string fileName = Path.GetFileName(archivo.FileName);

                        string folderPath = _configuration["PathFolder:value"];
                        string extensionArchivo = Path.GetExtension(archivo.FileName).ToLower();
                        string extensionXlsx = _configuration["TipoExcel"];
                        string extensionXls = _configuration["TipoXls"];
                        string extensionCsv = _configuration["TipoCsv"];

                        if (extensionArchivo == extensionXlsx || extensionArchivo == extensionXls || extensionArchivo == extensionCsv)
                        {
                            string filePath = Path.Combine(_environment.ContentRootPath, folderPath, Path.GetFileNameWithoutExtension(fileName)) + '-' + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";

                            if (!System.IO.File.Exists(filePath))
                            {
                                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                                {
                                    archivo.CopyTo(stream);
                                }

                                string connectionString = _configuration["ConnectionStringExcel:value"] + filePath;
                                ML.Result resultUsuarios = BL.Usuario.ConvertXSLXtoDataTable(connectionString);

                                if (resultUsuarios.Correct)
                                {
                                    ML.Result resultValidacion = BL.Usuario.ValidarExcel(resultUsuarios.Objects);
                                    HttpContext.Session.SetString("Display", "block");
                                    HttpContext.Session.SetString("File", fileName);
                                    if (resultValidacion.Objects.Count == 0)
                                    {
                                        resultValidacion.Correct = true;
                                        
                                        HttpContext.Session.SetString("PathArchivo", filePath);
                                        HttpContext.Session.SetString("Correct", "true");
                                    }
                                    else
                                    {
                                        resultValidacion.Correct = false;
                                        HttpContext.Session.SetString("Correct", "false");
                                        HttpContext.Session.SetString("PathArchivo", "error");                                       
                                    }                                  
                                                                 
                                    ViewBag.File = fileName;
                                    
                                    //return RedirectToAction("Index", resultValidacion);                                    
                                    return View("Index", resultValidacion);                                    
                                }
                                else
                                {
                                    ViewBag.Message = "El excel no contiene registros";
                                }
                            }
                        }
                        else
                        {
                            ViewBag.Message = "Favor de seleccionar un archivo .xlsx,.xls o .cvs. Verifique su archivo";
                        }
                    }
                    else
                    {
                        ViewBag.Message = "No selecciono ningun archivo, Seleccione uno correctamente";
                    }
                }
                else
                {
                    ViewBag.Message = "No seleccionó ningún archivo, intente de nuevo.";
                }
                
            }
            else
            {
                string rutaArchivoExcel = HttpContext.Session.GetString("PathArchivo");
                string connectionString = _configuration["ConnectionStringExcel:value"] + rutaArchivoExcel;

                ML.Result resultData = BL.Usuario.ConvertXSLXtoDataTable(connectionString);
                if (resultData.Correct)
                {
                    ML.Result resultErrores = new ML.Result();
                    resultErrores.Objects = new List<object>();

                    foreach (ML.Usuario usuarioItem in resultData.Objects)
                    {

                        ML.Result resultAdd = BL.Usuario.Add(usuarioItem);
                        if (!resultAdd.Correct)
                        {
                            resultErrores.Objects.Add("No se insertó el Usuario con UserName: " + usuarioItem.UserName + 
                                ", Nombre:" + usuarioItem.Nombre + ", Apellido Paterno:" + usuarioItem.ApellidoPaterno + 
                                ", Apellido Materno. " + usuarioItem.ApellidoMaterno + "Error: " + resultAdd.ErrorMessage);
                        }
                    }
                    if (resultErrores.Objects.Count > 0)
                    {

                        string fileError = Path.Combine(_environment.WebRootPath, @"~\Files\logErrores.txt");
                        using (StreamWriter writer = new StreamWriter(fileError))
                        {
                            foreach (string ln in resultErrores.Objects)
                            {
                                writer.WriteLine(ln);
                            }
                        }
                        ViewBag.Message = "Los usuarios no han sido registrados correctamente, verifique el contenido del archivo.";
                    }
                    else
                    {
                        HttpContext.Session.SetString("Display", "none");
                        HttpContext.Session.SetString("File", "");
                        HttpContext.Session.SetString("Correct", "");
                        HttpContext.Session.Remove("PathArchivo");
                        ViewBag.Message = "Los usuarios han sido registrados correctamente";
                    }

                }

            }
            return PartialView("Modal");
        }

    }//Controller
}//NS


