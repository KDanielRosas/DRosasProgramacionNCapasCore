using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Area
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.DrosasProgramacionNcapasContext context = new DL.DrosasProgramacionNcapasContext())
                {
                    var query = context.Areas.FromSqlRaw("AreaGetAll").ToList();

                    if (query != null)
                    {
                        result.Objects = new List<object>();

                        foreach (var item in query)
                        {
                            ML.Area area = new ML.Area();

                            area.IdArea = item.IdArea;
                            area.Nombre = item.Nombre;

                            result.Objects.Add(area);
                        }//foreach
                        result.Correct = true;
                    }//if
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Error al mostrar los registros";
                    }//else
                }//usingContext
            }//try
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                throw;
            }//catch

            return result;
        }//GetAll
    }//ClassArea
}
