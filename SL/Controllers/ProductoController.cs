using Microsoft.AspNetCore.Mvc;

namespace SL.Controllers
{
    public class ProductoController : Controller
    {
        [HttpGet]
        [Route("api/Producto/GetAll")]
        public ActionResult Index()
        {
            ML.Producto producto = new ML.Producto();
            ML.Result result = BL.Producto.GetAll(producto);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result);
            }
        }//GetAll

        [HttpPost]
        [Route("api/Producto/Add")]
        public ActionResult Add([FromBody] ML.Producto producto)
        {
            ML.Result result = BL.Producto.Add(producto);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return NotFound(result);
            }
        }//Add

        [HttpPost]
        [Route("api/Producto/Delete")]
        public ActionResult Delete(int idProducto)
        {
            ML.Result result = BL.Producto.Delete(idProducto);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }//Delete

        [HttpGet]
        [Route("api/Producto/GetById")]
        public ActionResult GetById(int idProducto)
        {
            ML.Result result = BL.Producto.GetById(idProducto);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }//GetById

        [HttpPost]
        [Route("api/Producto/Update")]
        public ActionResult Update([FromBody] ML.Producto producto)
        {
            ML.Result result = BL.Producto.Update(producto);
            if (result.Correct)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }//Update
    }
}
