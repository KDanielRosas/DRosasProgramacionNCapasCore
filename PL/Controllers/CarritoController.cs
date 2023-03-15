using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class CarritoController : Controller
    {
        public IActionResult Index()
        {
            string usuario = HttpContext.Session.GetString("Usuario");
            
            return View(model: usuario);
        }

        public IActionResult SaveName(string name)
        {
            ML.Usuario usuario = new ML.Usuario();
            usuario.Nombre = name;
            HttpContext.Session.SetString("Usuario", usuario.Nombre);
            
            return RedirectToAction("Index");
        }
    }
}

