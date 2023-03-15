using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Producto
    {
        public static ML.Result GetAll(ML.Producto producto)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.DrosasProgramacionNcapasContext context = new DL.DrosasProgramacionNcapasContext())
                {
                    var query = context.Productos.FromSqlRaw($"ProductoGetAll '{producto.Nombre}', {producto.IdDepartamento}").ToList();

                    if (query != null)
                    {
                        result.Objects = new List<object>();

                        foreach (var item in query)
                        {
                            producto = new ML.Producto();

                            producto.IdProducto = item.IdProducto;
                            producto.Nombre = item.Nombre;
                            producto.PrecioUnitario = item.PrecioUnitario;
                            producto.Stock = item.Stock;
                            producto.Proveedor = new ML.Proveedor();
                            producto.Proveedor.IdProveedor = item.IdProveedor.Value;
                            producto.Proveedor.Nombre = item.NombreProveedor;
                            producto.Departamento = new ML.Departamento();
                            producto.Departamento.IdDepartamento = item.IdDepartamento.Value;
                            producto.Departamento.Nombre = item.DepartamentoNombre;
                            producto.Departamento.Area = new ML.Area();
                            producto.Departamento.Area.IdArea = item.IdArea;
                            producto.Departamento.Area.Nombre = item.NombreArea;
                            producto.Descripcion = item.Descripcion;
                            producto.Imagen = item.Imagen;

                            result.Objects.Add(producto);
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

        public static ML.Result GetById(int idProducto)
        {
            ML.Result result = new ML.Result();
            ML.Producto producto = new ML.Producto();

            try
            {
                using (DL.DrosasProgramacionNcapasContext context = new DL.DrosasProgramacionNcapasContext())
                {
                    var query = context.Productos.FromSqlRaw($"ProductoGetById {idProducto}").AsEnumerable().FirstOrDefault();

                    if (query != null)
                    {
                        result.Objects = new List<object>();
                        result.Object = new object();
                        result.Correct = true;

                        producto.IdProducto = query.IdProducto;
                        producto.Nombre = query.Nombre;
                        producto.PrecioUnitario = query.PrecioUnitario;
                        producto.Stock = query.Stock;
                        producto.Proveedor = new ML.Proveedor();
                        producto.Proveedor.IdProveedor = query.IdProveedor.Value;
                        producto.Proveedor.Nombre = query.NombreProveedor;
                        producto.Departamento = new ML.Departamento();
                        producto.Departamento.IdDepartamento = query.IdDepartamento.Value;
                        producto.Departamento.Nombre = query.DepartamentoNombre;
                        producto.Departamento.Area = new ML.Area();
                        producto.Departamento.Area.IdArea = query.IdArea;
                        producto.Departamento.Area.Nombre = query.NombreArea;
                        producto.Descripcion = query.Descripcion;
                        producto.Imagen = query.Imagen;

                        result.Object = producto;
                        result.Objects.Add(producto);
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
        }//GetById

        public static ML.Result Add(ML.Producto producto)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.DrosasProgramacionNcapasContext context = new DL.DrosasProgramacionNcapasContext())
                {
                    int query = context.Database.ExecuteSqlRaw($"ProductoAdd '{producto.Nombre}', {producto.PrecioUnitario}, {producto.Stock}, " +
                        $"{producto.Proveedor.IdProveedor}, {producto.Departamento.IdDepartamento}, '{producto.Descripcion}', '{producto.Imagen}'");

                    if (query > 0)
                    {
                        result.Correct = true;
                    }//if
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Error al agregar";
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
        }//Add

        public static ML.Result Update(ML.Producto producto)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.DrosasProgramacionNcapasContext context = new DL.DrosasProgramacionNcapasContext())
                {
                    int query = context.Database.ExecuteSqlRaw($"ProductoUpdate {producto.IdProducto}, '{producto.Nombre}', " +
                        $"{producto.PrecioUnitario}, {producto.Stock}, {producto.Proveedor.IdProveedor}, {producto.Departamento.IdDepartamento}, " +
                        $"'{producto.Descripcion}', '{producto.Imagen}'");

                    if (query > 0)
                    {
                        result.Correct = true;
                    }//if
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Error al actualizar el registro";
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
        }//Update

        public static ML.Result Delete(int idProducto)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.DrosasProgramacionNcapasContext context = new DL.DrosasProgramacionNcapasContext())
                {
                    int query = context.Database.ExecuteSqlRaw($"ProductoDelete {idProducto}");

                    if (query > 0)
                    {
                        result.Correct = true;
                    }//if
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Error al eliminar";
                    }
                }//using
            }//try
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                throw;
            }//catch
            return result;
        }//Delete
    }
}
