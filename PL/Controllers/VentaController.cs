using DL;
using Microsoft.AspNetCore.Mvc;
using ML;

namespace PL.Controllers
{
    public class VentaController : Controller
    {
        // GET: Producto
        [HttpGet]
        public ActionResult ProductoGetAll()
        {
            ML.Producto producto = new ML.Producto();
            producto.Departamento = new ML.Departamento();
            producto.Departamento.Area = new ML.Area();

            ML.Result resultArea = BL.Area.GetAll();
            ML.Result result = BL.Producto.GetAll(producto);
            ML.Result resultDepartamento = BL.Departamento.GetByIdArea(producto.Departamento.Area.IdArea);
            
            producto.Productos = result.Objects;
            producto.Departamento.Departamentos = resultDepartamento.Objects;
            producto.Departamento.Area.Areas = resultArea.Objects;
                
            return View(producto);            
        }//GetAll

        [HttpPost]
        public ActionResult ProductoGetAll(ML.Producto producto)
        {
            producto.IdDepartamento = producto.Departamento.IdDepartamento;
            ML.Result result = BL.Producto.GetAll(producto);

                ML.Result resultArea = BL.Area.GetAll();                
                producto.Departamento.Area.Areas = resultArea.Objects;
                ML.Result resultDepartamento = BL.Departamento.GetByIdArea(producto.Departamento.Area.IdArea);
                producto.Departamento.Departamentos = resultDepartamento.Objects;

                producto.Productos = result.Objects;
                return View(producto);            
        }//GetAll

        [HttpGet]
        public ActionResult Carrito(int? idProducto)
        {
            ML.Producto producto = new ML.Producto();
            //GetById
            ML.Result result = BL.Producto.GetById(idProducto.Value);

            if (result.Correct)
            {
                producto.Proveedor = new ML.Proveedor();
                producto.Departamento = new ML.Departamento();
                producto.Departamento.Area = new ML.Area();                
                producto = (ML.Producto)result.Object;               

                ML.Result resultProveedor = BL.Proveedor.GetAll();
                ML.Result resultArea = BL.Area.GetAll();
                ML.Result resultDepartamento = BL.Departamento.GetByIdArea(producto.Departamento.Area.IdArea);

                producto.Proveedor.Proveedores = resultProveedor.Objects;
                producto.Departamento.Departamentos = resultDepartamento.Objects;
                producto.Departamento.Area.Areas = resultArea.Objects;

                producto.Productos = result.Objects;

                //Update
                return View(producto);
            }//if
            else
            {
                ViewBag.Message = "Error al mostrar la información del producto";
                return View("Modal");
            }//else
        }//Carrito

        [HttpGet]
        public ActionResult ProductoDelete(int idProducto)
        {
            ML.Result result = BL.Producto.Delete(idProducto);
            if (result.Correct)
            {
                ViewBag.Message = "Registro eliminado correctamente!";
            }
            else
            {
                ViewBag.Message = "Error al eliminar el registro...";
            }
            return View("Modal");
        }//Delete

        public ActionResult SumarCantidad(int idProducto)
        {
            ML.VentaProducto venta = new ML.VentaProducto();
            venta.VentaProductos = new List<object>();
            
            GetCarrito(venta);

            foreach (ML.Producto item in venta.VentaProductos)
            {
                if (item.IdProducto == idProducto)
                {
                    item.Cantidad += 1;
                }
            }
            HttpContext.Session.SetString("Producto", Newtonsoft.Json.JsonConvert.SerializeObject(venta.VentaProductos));
            return View("ResumenCompra", venta);
        }

        public ActionResult RestarCantidad(int idProducto)
        {
            ML.VentaProducto venta = new ML.VentaProducto();
            venta.VentaProductos = new List<object>();

            GetCarrito(venta);

            foreach (ML.Producto item in venta.VentaProductos)
            {
                if (item.IdProducto == idProducto)
                {
                    item.Cantidad -= 1;
                }
            }
            HttpContext.Session.SetString("Producto", Newtonsoft.Json.JsonConvert.SerializeObject(venta.VentaProductos));
            return View("ResumenCompra", venta);
        }

        public ActionResult Eliminar(int idProducto)
        {
            ML.VentaProducto venta = new ML.VentaProducto();
            venta.VentaProductos = new List<object>();

            GetCarrito(venta);

            var indice = 0; //variable para el index

            foreach (ML.Producto item in venta.VentaProductos)
            {// foreach que recorre el objeto venta producto
                if (item.IdProducto == idProducto)
                {// if que compara el id de el producto con el de ventaproducto

                    indice = venta.VentaProductos.IndexOf(item);//index 

                }
            }

            venta.VentaProductos.RemoveAt(indice);
            HttpContext.Session.SetString("Producto", Newtonsoft.Json.JsonConvert.SerializeObject(venta.VentaProductos));

            return View("ResumenCompra", venta);
        }

        public JsonResult ProductoAdd(int idProducto)
        {
            ML.Result result = BL.Producto.GetById(idProducto);

            return Json(result.Object);
        }

        public JsonResult DepartamentoGetByIdArea(int idArea)
        {
            ML.Result result = BL.Departamento.GetByIdArea(idArea);

            return Json(result.Objects);
        }//JsonResult

        public byte[] ImagenABase64(IFormFile foto)
        {
            using var fileStream = foto.OpenReadStream();

            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, (int)fileStream.Length);

            return bytes;
        }//imagenABase64

        [HttpGet]
        public ActionResult CartPost(int idProducto)
        {
            ML.Result result = BL.Producto.GetById(idProducto);
            ML.Producto producto = (ML.Producto)result.Object;
            bool existe = false;
            ML.VentaProducto ventaProducto = new ML.VentaProducto();
            ventaProducto.VentaProductos = new List<object>();

            if (HttpContext.Session.GetString("Producto") == null)
            {
                producto.Cantidad = producto.Cantidad = 1;
                producto.Subtotal = producto.PrecioUnitario * producto.Cantidad;
                ventaProducto.VentaProductos.Add(producto);
                HttpContext.Session.SetString("Producto", Newtonsoft.Json.JsonConvert.SerializeObject(ventaProducto.VentaProductos));
                var session = HttpContext.Session.GetString("Producto");
            }
            else
            {
                GetCarrito(ventaProducto);
                foreach (ML.Producto venta in ventaProducto.VentaProductos.ToList())
                {
                    if (producto.IdProducto == venta.IdProducto)
                    {
                        venta.Cantidad = venta.Cantidad + 1;   //True
                        venta.Subtotal = venta.PrecioUnitario * venta.Cantidad;
                        existe = true;
                    }
                    else
                    {
                        existe = false;
                    }
                    if (existe == true)
                    {
                        break;
                    }
                }
                if (existe == false)
                {
                    producto.Cantidad = producto.Cantidad = 1;
                    producto.Subtotal = producto.Cantidad * producto.PrecioUnitario;
                    ventaProducto.VentaProductos.Add(producto);
                }
                HttpContext.Session.SetString("Producto", Newtonsoft.Json.JsonConvert.SerializeObject(ventaProducto.VentaProductos));
            }
            if (HttpContext.Session.GetString("Producto") != null)
            {
                ViewBag.Message = "Se ha agregado el producto a tu carrito!";
                return PartialView("Modal");
            }
            else
            {
                ViewBag.Message = "No se pudo agregar tu producto ):";
                return PartialView("Modal");
            }

        }

        [HttpGet]
        public ActionResult ResumenCompra(ML.VentaProducto ventaProducto)
        {
            decimal costoTotal = 0;
            if (HttpContext.Session.GetString("Producto") == null)
            {
                return View();
            }
            else
            {
                ventaProducto.VentaProductos = new List<object>();
                GetCarrito(ventaProducto);
                ventaProducto.Total = costoTotal;
            }

            return View(ventaProducto);
        }
        public ML.VentaProducto GetCarrito(ML.VentaProducto ventaProducto)
        {
            var ventaSession = Newtonsoft.Json.JsonConvert.DeserializeObject<List<object>>(HttpContext.Session.GetString("Producto"));

            foreach (var obj in ventaSession)
            {
                ML.Producto objProducto = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Producto>(obj.ToString());
                ventaProducto.VentaProductos.Add(objProducto);
            }
            return ventaProducto;
        }
    }//Controller
}//NS
