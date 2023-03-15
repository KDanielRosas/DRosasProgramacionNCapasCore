using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Pais
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.DrosasProgramacionNcapasContext context = new DL.DrosasProgramacionNcapasContext())
                {
                    var query = context.Pais.FromSqlRaw("PaisGetAll").ToList();

                    if (query != null)
                    {
                        result.Objects = new List<object>();

                        foreach (var item in query)
                        {
                            ML.Pais pais = new ML.Pais();

                            pais.IdPais = item.IdPais;
                            pais.Nombre = item.Nombre;

                            result.Objects.Add(pais);
                        }//foreach
                        result.Correct = true;
                    }//if
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
    }//ClassPais
}
