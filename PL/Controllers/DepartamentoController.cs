using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class DepartamentoController : Controller
    {
        [HttpGet]
        public ActionResult GetAll()
        {
            ML.Result result = BL.Departamento.GetAll();
            ML.Departamento departamento = new ML.Departamento();
            if (result.Correct)
            {
                departamento.Departamentos = result.Objects;
                return View(departamento);
            }
            else
            {
                return View(departamento);
            }
        }//GetAll

        [HttpGet]
        public ActionResult Form(int? idDepartamento)
        {
            ML.Result resultArea = BL.Area.GetAll();
            ML.Departamento departamento = new ML.Departamento();
            departamento.Area = new ML.Area();

            if (resultArea.Correct)
            {
                departamento.Area.Areas = resultArea.Objects;
            }//if

            if (idDepartamento == null)
            {
                //Add FormVacio
                return View(departamento);
            }//if
            else
            {
                //GetById
                ML.Result result = BL.Departamento.GetById(idDepartamento.Value);

                if (result.Correct)
                {
                    departamento = (ML.Departamento)result.Object;
                    departamento.Area.Areas = resultArea.Objects;

                    //Update
                    return View(departamento);
                }//if
                else
                {
                    ViewBag.Message = "Error al mostrar la información.";
                    return View("Modal");
                }//else                
            }//else
        }//Form

        [HttpPost]
        public ActionResult Form(ML.Departamento departamento)
        {
            ML.Result result = new ML.Result();
            if (departamento.IdDepartamento == 0)
            {
                //Add
                result = BL.Departamento.Add(departamento);
                if (result.Correct)
                {
                    ViewBag.Message = "¡Se registró correctamente!";
                }
                else
                {
                    ViewBag.Message = "Error!";
                }
                return View("Modal");
            }
            else
            {
                //Update
                result = BL.Departamento.Update(departamento);
                if (result.Correct)
                {
                    ViewBag.Message = "¡Se actualizó correctamente el registro!";
                }
                else
                {
                    ViewBag.Message = "Error al actualizar el registro...";
                }
                return View("Modal");
            }
        }//Form

        [HttpGet]
        public ActionResult Delete(int idDepartamento)
        {
            ML.Result result = BL.Departamento.Delete(idDepartamento);
            if (result.Correct)
            {
                ViewBag.Message = "Registro eliminado correctamente";
            }
            else
            {
                ViewBag.Message = "Error al eliminar el registro.";
            }
            return View("Modal");
        }
    }//ClassController
}//NS