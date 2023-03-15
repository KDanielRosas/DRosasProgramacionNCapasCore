using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Proveedor
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.DrosasProgramacionNcapasContext context = new DL.DrosasProgramacionNcapasContext())
                {
                    var query = context.Proveedors.FromSqlRaw("ProveedorGetAll").ToList();

                    if (query != null)
                    {
                        result.Objects = new List<object>();
                        foreach (var item in query)
                        {
                            ML.Proveedor proveedor = new ML.Proveedor();

                            proveedor.IdProveedor = item.IdProveedor;
                            proveedor.Telefono = item.Telefono;
                            proveedor.Nombre = item.Nombre;

                            result.Objects.Add(proveedor);
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

        public static ML.Result GetById(int idProveedor)
        {
            ML.Result result = new ML.Result();
            ML.Proveedor proveedor = new ML.Proveedor();

            try
            {
                using (DL.DrosasProgramacionNcapasContext context = new DL.DrosasProgramacionNcapasContext())
                {
                    var query = context.Proveedors.FromSqlRaw($"ProveedorGetById {idProveedor}").AsEnumerable().FirstOrDefault();

                    if (query != null)
                    {
                        result.Objects = new List<object>();
                        result.Correct = true;

                        proveedor.IdProveedor = query.IdProveedor;
                        proveedor.Telefono = query.Telefono;
                        proveedor.Nombre = query.Nombre;

                        result.Objects.Add(proveedor);
                    }//if
                }//usingContext
            }//try
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                throw;
            }
            return result;
        }//GetById
    }//Proveedor
}
