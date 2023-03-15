using Microsoft.EntityFrameworkCore;
using ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Departamento
    {
        //EF
        public static ML.Result Add(ML.Departamento departamento)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.DrosasProgramacionNcapasContext context = new DL.DrosasProgramacionNcapasContext())
                {
                    int query = context.Database.ExecuteSqlRaw($"DepartamentoAdd '{departamento.Nombre}', {departamento.Area.IdArea}");

                    //Validar si hay filas afectadas
                    if (query > 0)
                    {
                        result.Correct = true;
                    }//if
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Error al registrar el departamento!";
                    }
                }//usingContext
            }//try
            catch (Exception ex)
            {
                result.Ex = ex;
                result.ErrorMessage = "Ocurrio un error al registrar el departamento!";
                result.Correct = false;
                throw;
            }//catch

            return result;

        }//AddEF

        public static ML.Result Update(ML.Departamento departamento)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.DrosasProgramacionNcapasContext context = new DL.DrosasProgramacionNcapasContext())
                {
                    int query = context.Database.ExecuteSqlRaw($"DepartamentoUpdate {departamento.IdDepartamento}, " +
                        $"'{departamento.Nombre}', {departamento.Area.IdArea}");

                    if (query > 0)
                    {
                        result.Correct = true;
                    }//if
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Error al actualizar el registro!";
                    }//else
                }//usingContext
            }//try
            catch (Exception ex)
            {
                result.Ex = ex;
                result.Correct = false;
                result.ErrorMessage = "Ocurrio un error!";
                throw;
            }//catch

            return result;

        }//UpdateEF

        public static ML.Result Delete(int idDepartamento)
        {
            ML.Result result = new Result();

            try
            {
                using (DL.DrosasProgramacionNcapasContext context = new DL.DrosasProgramacionNcapasContext())
                {
                    int query = context.Database.ExecuteSqlRaw($"DepartamentoDelete {idDepartamento}");

                    if (query > 0)
                    {
                        result.Correct = true;
                    }//if
                    else
                    {
                        result.Correct = false;
                    }//else
                }//usingContext
            }//try
            catch (Exception ex)
            {
                result.Ex = ex;
                result.Correct = false;
                result.ErrorMessage = "Error al eliminar el usuario!";
                throw;
            }//catch

            return result;

        }//DeleteEf

        public static ML.Result GetAll()
        {
            ML.Result result = new Result();

            try
            {
                using (DL.DrosasProgramacionNcapasContext context = new DL.DrosasProgramacionNcapasContext())
                {
                    var query = context.Departamentos.FromSqlRaw("DepartamentoGetAll").ToList();

                    if (query != null)
                    {
                        result.Objects = new List<object>();

                        foreach (var item in query)
                        {
                            ML.Departamento departamento = new ML.Departamento();

                            departamento.IdDepartamento = item.IdDepartamento;
                            departamento.Nombre = item.Nombre;
                            departamento.Area = new ML.Area();
                            departamento.Area.IdArea = item.IdArea.Value;
                            departamento.Area.Nombre = item.NombreArea;

                            result.Objects.Add(departamento);
                        }//foreach
                        result.Correct = true;
                    }//if
                }//usingContext
            }//try
            catch (Exception ex)
            {
                result.Ex = ex;
                result.Correct = false;
                result.ErrorMessage = "Error al mostrar los registros!";
                throw;
                throw;
            }//catch
            return result;
        }//GetAllEF

        public static ML.Result GetById(int idDepartamento)
        {
            ML.Result result = new ML.Result();
            ML.Departamento departamento = new ML.Departamento();

            try
            {
                using (DL.DrosasProgramacionNcapasContext context = new DL.DrosasProgramacionNcapasContext())
                {
                    var query = context.Departamentos.FromSqlRaw($"DepartamentoGetById {idDepartamento}").AsEnumerable().FirstOrDefault();

                    if (query != null)
                    {

                        result.Object = new object();
                        result.Correct = true;

                        departamento.IdDepartamento = query.IdDepartamento;
                        departamento.Nombre = query.Nombre;
                        departamento.Area = new ML.Area();
                        departamento.Area.IdArea = query.IdArea.Value;
                        departamento.Area.Nombre = query.NombreArea;

                        result.Object = departamento;

                    }//if
                }//usingContext
            }//try
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.ErrorMessage = "No existe un registro con ese ID.";
                throw;
                throw;
            }//catch

            return result;
        }//GetByIdEF

        public static ML.Result GetByIdArea(int idArea)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.DrosasProgramacionNcapasContext context = new DL.DrosasProgramacionNcapasContext())
                {
                    var query = context.Departamentos.FromSqlRaw($"DepartamentoGetByIdArea {idArea}").ToList();

                    if (query != null)
                    {
                        result.Objects = new List<object>();
                        foreach (var item in query)
                        {
                            ML.Departamento departamento = new ML.Departamento();

                            departamento.IdDepartamento = item.IdDepartamento;
                            departamento.Nombre = item.Nombre;
                            departamento.Area = new ML.Area();
                            departamento.Area.IdArea = item.IdArea.Value;
                            departamento.Area.Nombre = item.NombreArea;                            

                            result.Objects.Add(departamento);
                        }//foreach
                        result.Correct = true;
                    }//else
                }//using
            }//try
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                throw;
            }//catch
            return result;
        }//GetByIdArea
    }//ClassDepartamento
}
