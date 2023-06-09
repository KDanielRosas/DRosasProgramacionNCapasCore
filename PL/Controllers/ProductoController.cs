﻿using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class ProductoController : Controller
    {
        //Inyeccion de dependencias-- patron de diseño
        private readonly IConfiguration _configuration;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;

        public ProductoController(IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }
        // GET: Producto
        [HttpGet]
        public ActionResult GetAll()
        {
            ML.Producto producto = new ML.Producto();

                //Sin ServicioWeb
            /*ML.Result result = BL.Producto.GetAll(producto);

            if (result.Correct)
            {
                producto.Productos = result.Objects;
                return View(producto);
            }
            else
            {
                return View(producto);
            }*/

            ML.Result result = new ML.Result();
            result.Objects = new List<object>();

                //Con ServicioWeb
            try
            {

                using (var client = new HttpClient())
                {
                    string urlApi = _configuration["urlApi"];
                    client.BaseAddress = new Uri(urlApi);

                    var responseTask = client.GetAsync("Producto/GetAll");
                    responseTask.Wait();

                    var resultServicio = responseTask.Result;

                    if (resultServicio.IsSuccessStatusCode)
                    {
                        var readTask = resultServicio.Content.ReadAsAsync<ML.Result>();
                        readTask.Wait();

                        foreach (var resultItem in readTask.Result.Objects)
                        {
                            ML.Producto resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Producto>(resultItem.ToString());
                            result.Objects.Add(resultItemList);
                        }
                    }
                    producto.Productos = result.Objects;
                }

            }
            catch (Exception ex)
            {
                ViewBag.Message = "Error al mostrar los datos. " + ex.Message;
                return PartialView("Modal");
            }

            return View(producto);
        }//GetAll

        [HttpGet]
        public ActionResult Form(int? idProducto)
        {
            ML.Result resultProveedor = BL.Proveedor.GetAll();
            ML.Result resultDepartamento = BL.Departamento.GetAll();
            ML.Result resultArea = BL.Area.GetAll();

            ML.Producto producto = new ML.Producto();

            producto.Departamento = new ML.Departamento();
            producto.Proveedor = new ML.Proveedor();
            producto.Departamento.Area = new ML.Area();

            if (resultProveedor.Correct && resultDepartamento.Correct)
            {
                producto.Departamento.Departamentos = resultDepartamento.Objects;
                producto.Proveedor.Proveedores = resultProveedor.Objects;
                producto.Departamento.Area.Areas = resultArea.Objects;
            }//if

            if (idProducto == null)
            {
                //Add Form Vacio
                return View(producto);

            }//if
            else
            {
                //GetById Sin ServiciosWeb
                //ML.Result result = BL.Producto.GetById(idProducto.Value);

                //GetById Con ServicioWeb
                ML.Result result = new ML.Result();
                result.Object = new object();
                using (var client = new HttpClient())
                {
                    string urlApi = _configuration["urlApi"];
                    client.BaseAddress = new Uri(urlApi);

                    var responseTask = client.GetAsync("Producto/GetById?idProducto=" + idProducto);
                    responseTask.Wait();

                    var resultServicio = responseTask.Result;

                    if (resultServicio.IsSuccessStatusCode)
                    {
                        var readTask = resultServicio.Content.ReadAsAsync<ML.Result>();
                        readTask.Wait();
                        var resultItem = readTask.Result.Object;

                        ML.Producto resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Producto>(resultItem.ToString());
                        result.Object = resultItemList;

                        result.Correct = true;
                    }
                }

                if (result.Correct)
                {
                    producto = (ML.Producto)result.Object;

                    resultProveedor = BL.Proveedor.GetAll();
                    resultDepartamento = BL.Departamento.GetByIdArea(producto.Departamento.Area.IdArea);

                    producto.Proveedor.Proveedores = resultProveedor.Objects;
                    producto.Departamento.Departamentos = resultDepartamento.Objects;
                    producto.Departamento.Area.Areas = resultArea.Objects;

                    //Update
                    return View(producto);
                }//if
                else
                {
                    ViewBag.Message = "Error al mostrar la información del producto";
                    return View("Modal");
                }//else
            }//else
        }//Form

        [HttpPost]
        public ActionResult Form(ML.Producto producto)
        {
            //Sin ServicioWeb
            /*ML.Result result = new ML.Result();
            IFormFile file = Request.Form.Files["fuImage"];

            if (file != null)
            {
                byte[] imagen = ImagenABase64(file);
                producto.Imagen = Convert.ToBase64String(imagen);
            }

            if (producto.IdProducto == 0)
            {
                //Add
                result = BL.Producto.Add(producto);

                if (result.Correct)
                {
                    ViewBag.Message = "¡Producto registrado correctamente!";
                }
                else
                {
                    ViewBag.Message = "Error al registrar el producto...";
                }
                return View("Modal");
            }//if
            else
            {
                //Update
                result = BL.Producto.Update(producto);
                if (result.Correct)
                {
                    ViewBag.Message = "¡Se actualizó el registro correctamente!";
                }//if
                else
                {
                    ViewBag.Message = "Error al actualizar el registo...";
                }//else
                return View("Modal");
            }*/

            //Con ServicioWeb
            IFormFile file = Request.Form.Files["fuImage"];

            if (file != null)
            {
                byte[] imagen = ImagenABase64(file);

                producto.Imagen = Convert.ToBase64String(imagen);
            }
            //Add
            if (producto.IdProducto == 0)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_configuration["urlApi"]);

                    //HTTP POST
                    var postTask = client.PostAsJsonAsync("Producto/Add", producto);
                    postTask.Wait();

                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        ViewBag.Message = "Se ha ingresado el registro.";
                    }
                    else
                    {
                        ViewBag.Message = "No se ha ingresado el registro.";
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
                    var postTask = client.PostAsJsonAsync("Producto/Update", producto);
                    postTask.Wait();

                    var result = postTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        ViewBag.Message = "Se ha actualizado el producto.";
                        return PartialView("Modal");
                    }
                    else
                    {
                        ViewBag.Message = "No se ha actualizado el producto.";
                        return PartialView("Modal");
                    }
                }
            }
        }//Form

        [HttpGet]
        public ActionResult Delete(int idProducto)
        {
            //Sin ServicioWeb
            /*ML.Result result = BL.Producto.Delete(idProducto);
            if (result.Correct)
            {
                ViewBag.Message = "Registro eliminado correctamente!";
            }
            else
            {
                ViewBag.Message = "Error al eliminar el registro...";
            }
            return View("Modal");*/

            //Con ServicioWeb
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_configuration["urlApi"]);

                //HTTP POST
                var deleteTask = client.PostAsync("Producto/Delete?idProducto=" + idProducto, null);
                deleteTask.Wait();

                var result = deleteTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    ViewBag.Message = "Se ha eliminado el producto.";
                    return PartialView("Modal");
                }
                else
                {
                    ViewBag.Message = "No se ha eliminado el producto.";
                    return PartialView("Modal");
                }
            }

        }//Delete

        public JsonResult DepartamentoGetByIdArea(int idArea)
        {
            ML.Result result = BL.Departamento.GetByIdArea(idArea);

            return Json(result.Objects);
        }

        public byte[] ImagenABase64(IFormFile foto)
        {
            using var fileStream = foto.OpenReadStream();

            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, (int)fileStream.Length);

            return bytes;
        }//imagenABase64
    }
}