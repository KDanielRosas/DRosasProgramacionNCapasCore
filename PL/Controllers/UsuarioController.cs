using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class UsuarioController : Controller
    {
        //Inyeccion de dependencias-- patron de diseño
        private readonly IConfiguration _configuration;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;

        public UsuarioController(IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }
        [HttpGet]
        public ActionResult GetAll()
        {
            ML.Usuario usuario = new ML.Usuario();
                //Sin ServiciosWeb
            /*ML.Result result = BL.Usuario.GetAll(usuario);

            if (result.Correct)
            {
                usuario.Usuarios = result.Objects;
                return View(usuario);
            }
            else
            {
                return View(usuario);
            }*/

                //Con ServiciosWeb
            ML.Result result = new ML.Result();
            result.Objects = new List<object>();

            try
            {

                using (var client = new HttpClient())
                {
                    string urlApi = _configuration["urlApi"];
                    client.BaseAddress = new Uri(urlApi);

                    var responseTask = client.GetAsync("Usuario/GetAll");
                    responseTask.Wait();

                    var resultServicio = responseTask.Result;

                    if (resultServicio.IsSuccessStatusCode)
                    {
                        var readTask = resultServicio.Content.ReadAsAsync<ML.Result>();
                        readTask.Wait();

                        foreach (var resultItem in readTask.Result.Objects)
                        {
                            ML.Usuario resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Usuario>(resultItem.ToString());
                            result.Objects.Add(resultItemList);
                        }
                    }
                    usuario.Usuarios = result.Objects;
                }

            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error al mostrar los registros. " + ex.Message;
                return PartialView("Modal");
            }

            return View(usuario);
        }//GetAll

        [HttpPost]
        public ActionResult GetAll(ML.Usuario usuario)
        {
            ML.Result result = BL.Usuario.GetAll(usuario);

            if (result.Correct)
            {
                usuario.Usuarios = result.Objects;
                return View(usuario);
            }
            else
            {
                return View(usuario);
            }
        }//GetAll

        [HttpGet]
        public ActionResult Form(int? idUsuario)
        {
            ML.Result resultRol = BL.Rol.GetAll();
            ML.Result resultPais = BL.Pais.GetAll();

            ML.Usuario usuario = new ML.Usuario();

            usuario.Rol = new ML.Rol();
            usuario.Direccion = new ML.Direccion();
            usuario.Direccion.Colonia = new ML.Colonia();
            usuario.Direccion.Colonia.Municipio = new ML.Municipio();
            usuario.Direccion.Colonia.Municipio.Estado = new ML.Estado();
            usuario.Direccion.Colonia.Municipio.Estado.Pais = new ML.Pais();

            if (resultRol.Correct && resultPais.Correct)
            {
                usuario.Rol.Roles = resultRol.Objects;
                usuario.Direccion.Colonia.Municipio.Estado.Pais.Paises = resultPais.Objects;

            }//if

            if (idUsuario == null)
            {
                //Add FormVacio

                return View(usuario);
            }//if
            else
            {
                //GetById Sin ServiciosWeb
                //ML.Result result = BL.Usuario.GetById(idUsuario.Value);

                //GetById Con ServiciosWeb
                ML.Result result = new ML.Result();
                result.Object = new object();
                using (var client = new HttpClient())
                {
                    string urlApi = _configuration["urlApi"];
                    client.BaseAddress = new Uri(urlApi);

                    var responseTask = client.GetAsync("Usuario/GetById?idUsuario=" + idUsuario);
                    responseTask.Wait();

                    var resultServicio = responseTask.Result;

                    if (resultServicio.IsSuccessStatusCode)
                    {
                        var readTask = resultServicio.Content.ReadAsAsync<ML.Result>();
                        readTask.Wait();
                        var resultItem = readTask.Result.Object;
                        
                            ML.Usuario resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Usuario>(resultItem.ToString());
                            result.Object = resultItemList;
                        
                        result.Correct = true;
                    }                    
                }

                if (result.Correct)
                {
                    usuario = (ML.Usuario)result.Object;

                    ML.Result resultEstado = BL.Estado.GetByIdPais(usuario.Direccion.Colonia.Municipio.Estado.Pais.IdPais);
                    ML.Result resultMunicipio = BL.Municipio.GetByIdEstado(usuario.Direccion.Colonia.Municipio.Estado.IdEstado);
                    ML.Result resultColonia = BL.Colonia.GetByIdMunicipio(usuario.Direccion.Colonia.Municipio.IdMunicipio);

                    usuario.Rol.Roles = resultRol.Objects;
                    usuario.Direccion.Colonia.Colonias = resultColonia.Objects;
                    usuario.Direccion.Colonia.Municipio.Municipios = resultMunicipio.Objects;
                    usuario.Direccion.Colonia.Municipio.Estado.Estados = resultEstado.Objects;
                    usuario.Direccion.Colonia.Municipio.Estado.Pais.Paises = resultPais.Objects;

                    //Update                   
                    return View(usuario);
                }//if
                else
                {
                    ViewBag.Message = "Error al mostrar la información del usuario!";
                    return View("Modal");
                }//else
            }//else
        }//Form

        [HttpPost]
        public ActionResult Form(ML.Usuario usuario)
        {
            //Sin servicio Web
            /* ML.Result result = new ML.Result();
             IFormFile file = Request.Form.Files["fuImage"];

             if (file != null)
             {
                 byte[] imagen = ImagenABase64(file);
                 usuario.Imagen = Convert.ToBase64String(imagen);
             }

             //if (ModelState.IsValid == true)
             //{
             if (usuario.IdUsuario == 0)
             {
                 //Add

                 result = BL.Usuario.Add(usuario);

                 if (result.Correct)
                 {
                     ViewBag.Message = "Usuario registrado correctamente!";
                 }//if
                 else
                 {
                     ViewBag.Message = "Error al registrar al usuario...";
                 }//else
                 return View("Modal");
             }//if
             else
             {
                 //Update                
                 result = BL.Usuario.Update(usuario);
                 if (result.Correct)
                 {
                     ViewBag.Message = "Se actualizó el registro correctamente!";
                 }//if
                 else
                 {
                     ViewBag.Message = "Error al actualizar el registro...";
                 }//else
                 return View("Modal");
             }//else
              //}//ifModelState
              //else
              //{
              //    ML.Result resultRol = BL.Rol.GetAll();
              //    ML.Result resultPais = BL.Pais.GetAll();                

             //    usuario.Rol = new ML.Rol();
             //    usuario.Direccion = new ML.Direccion();
             //    usuario.Direccion.Colonia = new ML.Colonia();
             //    usuario.Direccion.Colonia.Municipio = new ML.Municipio();
             //    usuario.Direccion.Colonia.Municipio.Estado = new ML.Estado();
             //    usuario.Direccion.Colonia.Municipio.Estado.Pais = new ML.Pais();

             //    usuario.Rol.Roles = resultRol.Objects;
             //    usuario.Direccion.Colonia.Municipio.Estado.Pais.Paises = resultPais.Objects;

             //    ML.Result resultEstado = BL.Estado.GetByIdPais(usuario.Direccion.Colonia.Municipio.Estado.Pais.IdPais);
             //    ML.Result resultMunicipio = BL.Municipio.GetByIdEstado(usuario.Direccion.Colonia.Municipio.Estado.IdEstado);
             //    ML.Result resultColonia = BL.Colonia.GetByIdMunicipio(usuario.Direccion.Colonia.Municipio.IdMunicipio);

             //    usuario.Direccion.Colonia.Colonias = resultColonia.Objects;
             //    usuario.Direccion.Colonia.Municipio.Municipios = resultMunicipio.Objects;
             //    usuario.Direccion.Colonia.Municipio.Estado.Estados = resultEstado.Objects;
             //    usuario.Direccion.Colonia.Municipio.Estado.Pais.Paises = resultPais.Objects;

             //return View(usuario);
             //}//elseModelState
             */

            //Con servicio Web
            IFormFile file = Request.Form.Files["fuImage"];

            if (file != null)
            {
                byte[] imagen = ImagenABase64(file);

                usuario.Imagen = Convert.ToBase64String(imagen);
            }
            //Add
            if (usuario.IdUsuario == 0)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["urlApi"]);

                    //HTTP POST
                    var postTask = client.PostAsJsonAsync<ML.Usuario>("Usuario/Add", usuario);
                    postTask.Wait();

                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        ViewBag.Message = "Se ha actualizado el registro";                        
                    }
                    else
                    {
                        ViewBag.Message = "No se ha actualizado el usuario";                        
                    }
                    return PartialView("Modal");
                }//using
            }//if

            //Update
            else
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["urlApi"]);

                    //HTTP POST
                    var postTask = client.PostAsJsonAsync<ML.Usuario>("Usuario/Update", usuario);
                    postTask.Wait();

                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        ViewBag.Message = "Se ha actualizado el usuario.";
                        return PartialView("Modal");
                    }
                    else
                    {
                        ViewBag.Message = "No se ha actualizado el usuario.";
                        return PartialView("Modal");
                    }
                }
            }
        }//Form

        [HttpGet]
        public ActionResult Delete(int idUsuario)
        {
            //Sin Servicio Web
            /*ML.Result result = BL.Usuario.Delete(idUsuario);
            if (result.Correct)
            {
                ViewBag.Message = "Registro eliminado correctamente!";
            }
            else
            {
                ViewBag.Message = "Error al eliminar el registro...";
            }
            return View("Modal");*/

            //Con Servicio Web
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration["urlApi"]);                

                //HTTP POST
                var deleteTask = client.PostAsync("Usuario/Delete?idUsuario=" + idUsuario,null);
                deleteTask.Wait();

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    ViewBag.Message = "Se ha eliminado a el usuario.";
                    return PartialView("Modal");
                }
                else
                {
                    ViewBag.Message = "No se ha eliminado a el usuario.";
                    return PartialView("Modal");
                }
            }
        }//Delete

        [HttpPost]
        public JsonResult EstadoGetByIdPais(int idPais)
        {
            ML.Result result = BL.Estado.GetByIdPais(idPais);

            return Json(result.Objects);
        }//EstadoGetByIdPais

        [HttpPost]
        public JsonResult MunicipioGetByIdEstado(int idEstado)
        {
            ML.Result result = BL.Municipio.GetByIdEstado(idEstado);

            return Json(result.Objects);
        }//MunicipioGetByIdEstado

        [HttpPost]
        public JsonResult ColoniaGetByIdMunicipio(int idMunicipio)
        {
            ML.Result result = BL.Colonia.GetByIdMunicipio(idMunicipio);

            return Json(result.Objects);
        }//ColoniaGetByIdMunicipio

        public byte[] ImagenABase64(IFormFile foto)
        {
            using var fileStream = foto.OpenReadStream();

            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, (int)fileStream.Length);

            return bytes;
        }//imagenABase64

        public JsonResult ChangeStatus(int idUsuario, bool status)
        {
            ML.Result result = BL.Usuario.ChangeStatus(idUsuario, status);
            return Json(result);
        }//ChangeStatus

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }//Login(get)

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            ML.Result result = BL.Usuario.GetByUserName(username, password);
            ML.Usuario usuario = (ML.Usuario)result.Object;

            if (result.Correct)
            {
                if (usuario.Password == password)
                {
                    return RedirectToAction("Index", "Home");
                }//if
                else
                {
                    ViewBag.Message = "Contraseña incorrecta";
                    return PartialView("Modal1");
                }//if
            }//if
            else
            {
                ViewBag.Message = "El usuario no existe";
                return PartialView("Modal1");
            }//else           
        }
    }//UsuarioController
}//NS
