using Microsoft.EntityFrameworkCore;
using System.Data.OleDb;
using System.Data;

namespace BL
{
    public class Usuario
    {
        public static ML.Result Add(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.DrosasProgramacionNcapasContext context = new DL.DrosasProgramacionNcapasContext())
                {
                    int query = context.Database.ExecuteSqlRaw($"UsuarioAdd '{usuario.UserName}', '{usuario.Nombre}', " +
                        $"'{usuario.ApellidoPaterno}', '{usuario.ApellidoMaterno}', '{usuario.Email}', '{usuario.Password}', " +
                        $"'{usuario.FechaNacimiento}', '{usuario.Sexo}', '{usuario.Telefono}', '{usuario.Celular}', " +
                        $"'{usuario.CURP}', '{usuario.Imagen}', {(byte)usuario.Rol.IdRol} , '{usuario.Direccion.Calle}', " +
                        $"'{usuario.Direccion.NumeroInterior}', '{usuario.Direccion.NumeroExterior}', '{usuario.Direccion.Colonia.IdColonia}'");

                    //Validar si hay filas afectadas
                    if (query > 0)
                    {
                        result.Correct = true;
                    }//if
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Error al registrar el alumno.";
                    }//else                  
                }//using SQL Connection
            }//try

            catch (Exception ex)
            {
                result.Ex = ex;
                result.Correct = false;
                result.ErrorMessage = "Ocurrio un error al registrar el usuario";
                throw;
            }//catch
            return result;
        }//Add

        public static ML.Result Update(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.DrosasProgramacionNcapasContext context = new DL.DrosasProgramacionNcapasContext())
                {
                    int queryUpdate = context.Database.ExecuteSqlRaw($"UsuarioUpdate {usuario.IdUsuario}, '{usuario.UserName}', " +
                        $"'{usuario.Nombre}', '{usuario.ApellidoPaterno}', '{usuario.ApellidoMaterno}', '{usuario.Email}', " +
                        $"'{usuario.Password}', '{usuario.FechaNacimiento}', '{usuario.Sexo}', '{usuario.Telefono}', '{usuario.Celular}', " +
                        $"'{usuario.CURP}', '{usuario.Imagen}', {(byte)usuario.Rol.IdRol}, '{usuario.Direccion.Calle}', " +
                        $"'{usuario.Direccion.NumeroInterior}', '{usuario.Direccion.NumeroExterior}', '{usuario.Direccion.Colonia.IdColonia}'");

                    //Validar si hay filas afectadas
                    if (queryUpdate > 0)
                    {
                        result.Correct = true;

                    }//if
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Error al actualizar el registro";
                    }//else

                }//using
            }//try

            catch (Exception ex)
            {
                result.Ex = ex;
                result.Correct = false;
                result.ErrorMessage = "Error al actualizar el usuario.";
                throw;
            }//catch
            return result;
        }//UpdateEF

        public static ML.Result Delete(int idUsuario)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.DrosasProgramacionNcapasContext context = new DL.DrosasProgramacionNcapasContext())
                {
                    int queryDelete = context.Database.ExecuteSqlRaw($"UsuarioDelete {idUsuario}");

                    //Validar si hay filas afectadas
                    if (queryDelete > 0)
                    {
                        result.Correct = true;
                    }//if
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "Error al eliminar el registro";
                    }//else
                }//using DRosas...
            }//try

            catch (Exception ex)
            {
                result.Ex = ex;
                result.Correct = false;
                result.ErrorMessage = "Ocurrio un error al eliminar el usuario";
                throw;
            }//catch
            return result;
        }//DeleteEF

        public static ML.Result GetAll(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();
            try
            {
                using (DL.DrosasProgramacionNcapasContext context = new DL.DrosasProgramacionNcapasContext())
                {
                    var queryGetAll = context.Usuarios.FromSqlRaw($"UsuarioGetAll '{usuario.Nombre}', " +
                        $"'{usuario.ApellidoPaterno}', '{usuario.ApellidoMaterno}'").ToList();

                    if (queryGetAll != null)
                    {
                        result.Objects = new List<object>();

                        foreach (var item in queryGetAll)
                        {
                            usuario = new ML.Usuario();

                            usuario.IdUsuario = item.IdUsuario;
                            usuario.UserName = item.UserName;
                            usuario.Nombre = item.Nombre;
                            usuario.ApellidoPaterno = item.ApellidoPaterno;
                            usuario.ApellidoMaterno = item.ApellidoMaterno;
                            usuario.Email = item.Email;
                            usuario.Password = item.Password;
                            usuario.FechaNacimiento = item.FechaNacimiento.ToString("dd/MM/yyyy");
                            usuario.Sexo = item.Sexo;
                            usuario.Telefono = item.Telefono;
                            usuario.Celular = item.Celular;
                            usuario.CURP = item.Curp;
                            usuario.Imagen = item.Imagen;
                            usuario.Rol = new ML.Rol();
                            usuario.Rol.IdRol = (byte)item.IdRol;
                            usuario.Rol.Nombre = item.NombreRol;
                            usuario.Direccion = new ML.Direccion();
                            usuario.Direccion.IdDireccion = item.IdDireccion;
                            usuario.Direccion.Calle = item.Calle;
                            usuario.Direccion.NumeroInterior = item.NumeroInterior;
                            usuario.Direccion.NumeroExterior = item.NumeroExterior;
                            usuario.Direccion.Colonia = new ML.Colonia();
                            usuario.Direccion.Colonia.IdColonia = item.IdColonia;
                            usuario.Direccion.Colonia.Nombre = item.NombreColonia;
                            usuario.Direccion.Colonia.Municipio = new ML.Municipio();
                            usuario.Direccion.Colonia.Municipio.IdMunicipio = item.IdMunicipio;
                            usuario.Direccion.Colonia.Municipio.Nombre = item.NombreMunicipio;
                            usuario.Direccion.Colonia.Municipio.Estado = new ML.Estado();
                            usuario.Direccion.Colonia.Municipio.Estado.IdEstado = item.IdEstado;
                            usuario.Direccion.Colonia.Municipio.Estado.Nombre = item.NombreEstado;
                            usuario.Direccion.Colonia.Municipio.Estado.Pais = new ML.Pais();
                            usuario.Direccion.Colonia.Municipio.Estado.Pais.IdPais = item.IdPais;
                            usuario.Direccion.Colonia.Municipio.Estado.Pais.Nombre = item.NombrePais;
                            usuario.Status = item.Status;

                            result.Objects.Add(usuario);
                        }//foreach
                        result.Correct = true;
                    }//if                                                   
                }//using context
            }//try
            catch (Exception ex)
            {
                result.Ex = ex;
                result.Correct = false;
                result.ErrorMessage = "Error al mostrar los registros!";
                throw;
            }
            return result;
        }//GetAllEF

        public static ML.Result GetById(int idUsuario)
        {
            ML.Result result = new ML.Result();
            ML.Usuario usuario = new ML.Usuario();
            try
            {
                using (DL.DrosasProgramacionNcapasContext context = new DL.DrosasProgramacionNcapasContext())
                {
                    var queryGetById = context.Usuarios.FromSqlRaw($"UsuarioGetById {idUsuario}").AsEnumerable().FirstOrDefault();

                    if (queryGetById != null)
                    {
                        result.Object = new object();
                        result.Correct = true;

                        usuario.IdUsuario = queryGetById.IdUsuario;
                        usuario.UserName = queryGetById.UserName;
                        usuario.Nombre = queryGetById.Nombre;
                        usuario.ApellidoPaterno = queryGetById.ApellidoPaterno;
                        usuario.ApellidoMaterno = queryGetById.ApellidoMaterno;
                        usuario.Email = queryGetById.Email;
                        usuario.Password = queryGetById.Password;
                        usuario.FechaNacimiento = queryGetById.FechaNacimiento.ToString("dd/MM/yyyy");
                        usuario.Sexo = queryGetById.Sexo;
                        usuario.Telefono = queryGetById.Telefono;
                        usuario.Celular = queryGetById.Celular;
                        usuario.CURP = queryGetById.Curp;
                        usuario.Imagen = queryGetById.Imagen;
                        usuario.Rol = new ML.Rol();
                        usuario.Rol.IdRol = (byte)queryGetById.IdRol;
                        usuario.Rol.Nombre = queryGetById.NombreRol;
                        usuario.Direccion = new ML.Direccion();
                        usuario.Direccion.IdDireccion = queryGetById.IdDireccion;
                        usuario.Direccion.Calle = queryGetById.Calle;
                        usuario.Direccion.NumeroInterior = queryGetById.NumeroInterior;
                        usuario.Direccion.NumeroExterior = queryGetById.NumeroExterior;
                        usuario.Direccion.Colonia = new ML.Colonia();
                        usuario.Direccion.Colonia.IdColonia = queryGetById.IdColonia;
                        usuario.Direccion.Colonia.Nombre = queryGetById.NombreColonia;
                        usuario.Direccion.Colonia.Municipio = new ML.Municipio();
                        usuario.Direccion.Colonia.Municipio.IdMunicipio = queryGetById.IdMunicipio;
                        usuario.Direccion.Colonia.Municipio.Nombre = queryGetById.NombreMunicipio;
                        usuario.Direccion.Colonia.Municipio.Estado = new ML.Estado();
                        usuario.Direccion.Colonia.Municipio.Estado.IdEstado = queryGetById.IdEstado;
                        usuario.Direccion.Colonia.Municipio.Estado.Nombre = queryGetById.NombreEstado;
                        usuario.Direccion.Colonia.Municipio.Estado.Pais = new ML.Pais();
                        usuario.Direccion.Colonia.Municipio.Estado.Pais.IdPais = queryGetById.IdPais;
                        usuario.Direccion.Colonia.Municipio.Estado.Pais.Nombre = queryGetById.NombrePais;

                        result.Object = usuario;
                    }//if                                                                                    
                }//using
            }//try

            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.ErrorMessage = "No existe un registro con ese ID.";
                throw;
            }//catch
            return result;
        }//GetById

        public static ML.Result ChangeStatus(int idUsuario, bool status) 
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.DrosasProgramacionNcapasContext context = new DL.DrosasProgramacionNcapasContext())
                {
                    int query = context.Database.ExecuteSqlRaw($"UsuarioChangeStatus {idUsuario}, {status}");

                    if (query > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
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
        }//ChangeStatus

        public static ML.Result GetByUserName(string userName, string password)
        {
            ML.Result result = new ML.Result();
            ML.Usuario usuario = new ML.Usuario();
            try
            {
                using (DL.DrosasProgramacionNcapasContext context = new DL.DrosasProgramacionNcapasContext())
                {
                    var queryGetById = context.Usuarios.FromSqlRaw($"UsuarioGetByUserName '{userName}', '{password}'").AsEnumerable().FirstOrDefault();

                    if (queryGetById != null)
                    {
                        result.Object = new object();
                        result.Correct = true;

                        usuario.IdUsuario = queryGetById.IdUsuario;
                        usuario.UserName = queryGetById.UserName;
                        usuario.Nombre = queryGetById.Nombre;
                        usuario.ApellidoPaterno = queryGetById.ApellidoPaterno;
                        usuario.ApellidoMaterno = queryGetById.ApellidoMaterno;
                        usuario.Email = queryGetById.Email;
                        usuario.Password = queryGetById.Password;
                        usuario.FechaNacimiento = queryGetById.FechaNacimiento.ToString("dd/MM/yyyy");
                        usuario.Sexo = queryGetById.Sexo;
                        usuario.Telefono = queryGetById.Telefono;
                        usuario.Celular = queryGetById.Celular;
                        usuario.CURP = queryGetById.Curp;
                        usuario.Imagen = queryGetById.Imagen;
                        usuario.Rol = new ML.Rol();
                        usuario.Rol.IdRol = (byte)queryGetById.IdRol;
                        usuario.Rol.Nombre = queryGetById.NombreRol;
                        usuario.Direccion = new ML.Direccion();
                        usuario.Direccion.IdDireccion = queryGetById.IdDireccion;
                        usuario.Direccion.Calle = queryGetById.Calle;
                        usuario.Direccion.NumeroInterior = queryGetById.NumeroInterior;
                        usuario.Direccion.NumeroExterior = queryGetById.NumeroExterior;
                        usuario.Direccion.Colonia = new ML.Colonia();
                        usuario.Direccion.Colonia.IdColonia = queryGetById.IdColonia;
                        usuario.Direccion.Colonia.Nombre = queryGetById.NombreColonia;
                        usuario.Direccion.Colonia.Municipio = new ML.Municipio();
                        usuario.Direccion.Colonia.Municipio.IdMunicipio = queryGetById.IdMunicipio;
                        usuario.Direccion.Colonia.Municipio.Nombre = queryGetById.NombreMunicipio;
                        usuario.Direccion.Colonia.Municipio.Estado = new ML.Estado();
                        usuario.Direccion.Colonia.Municipio.Estado.IdEstado = queryGetById.IdEstado;
                        usuario.Direccion.Colonia.Municipio.Estado.Nombre = queryGetById.NombreEstado;
                        usuario.Direccion.Colonia.Municipio.Estado.Pais = new ML.Pais();
                        usuario.Direccion.Colonia.Municipio.Estado.Pais.IdPais = queryGetById.IdPais;
                        usuario.Direccion.Colonia.Municipio.Estado.Pais.Nombre = queryGetById.NombrePais;

                        result.Object = usuario;
                    }//if                                                                                    
                }//using
            }//try

            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.ErrorMessage = "No existe un registro con ese ID.";
                throw;
            }//catch
            return result;
        }//GetById

        public static ML.Result ConvertXSLXtoDataTable(string connectionString)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (OleDbConnection context = new OleDbConnection(connectionString))
                {                                     
                    string query = "SELECT * FROM [Sheet1$]";
                    using (OleDbCommand cmd = new OleDbCommand())
                    {
                        cmd.CommandText = query;
                        cmd.Connection = context;


                        OleDbDataAdapter da = new OleDbDataAdapter();
                        da.SelectCommand = cmd;

                        DataTable tableUsuario = new DataTable();

                        da.Fill(tableUsuario);

                        if (tableUsuario.Rows.Count > 0)
                        {
                            result.Objects = new List<object>();

                            foreach (DataRow row in tableUsuario.Rows)
                            {
                                ML.Usuario usuario = new ML.Usuario();
                                usuario.UserName = row[0].ToString();
                                usuario.Nombre = row[1].ToString();
                                usuario.ApellidoPaterno = row[2].ToString();
                                usuario.ApellidoMaterno = row[3].ToString();
                                usuario.Email = row[4].ToString();
                                usuario.Password = row[5].ToString();
                                usuario.FechaNacimiento = row[6].ToString();
                                usuario.Sexo = row[7].ToString();
                                usuario.Telefono = row[8].ToString();
                                usuario.Celular = row[9].ToString();
                                usuario.CURP = row[10].ToString();
                                usuario.Rol = new ML.Rol();
                                usuario.Rol.IdRol = int.Parse(row[11].ToString());
                                usuario.Direccion = new ML.Direccion();
                                usuario.Direccion.Calle = row[12].ToString();
                                usuario.Direccion.NumeroInterior = row[13].ToString();
                                usuario.Direccion.NumeroExterior = row[14].ToString();
                                usuario.Direccion.Colonia = new ML.Colonia();
                                usuario.Direccion.Colonia.IdColonia = int.Parse(row[15].ToString());

                                result.Objects.Add(usuario);
                            }

                            result.Correct = true;

                        }

                        result.Object = tableUsuario;

                        if (tableUsuario.Rows.Count >= 1)
                        {
                            result.Correct = true;
                        }
                        else
                        {
                            result.Correct = false;
                            result.ErrorMessage = "No existen registros en el excel";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;

            }
            return result;
        }//ConvertXLSXToDataTable

        public static ML.Result ValidarExcel(List<object> Object)
        {
            ML.Result result = new ML.Result();

            try
            {
                result.Objects = new List<object>();
                //DataTable  //Rows //Columns
                int i = 1;
                foreach (ML.Usuario usuario in Object)
                {
                    ML.ErrorExcel error = new ML.ErrorExcel();
                    error.IdRegistro = i++;

                    if (usuario.UserName == "")
                    {
                        error.Mensaje += "Ingresar el UserName  ";
                    }
                    if (usuario.Nombre == "")
                    {
                        error.Mensaje += "Ingresar el nombre  ";
                    }
                    if (usuario.ApellidoPaterno == "")
                    {
                        error.Mensaje += "Ingresar el apellido paterno ";
                    }
                    if (usuario.ApellidoMaterno == "")
                    {
                        error.Mensaje += "Ingresar el apellido materno";
                    }
                    if (usuario.Email == "")
                    {
                        error.Mensaje += "Ingresar el email ";
                    }
                    if (usuario.Password == "")
                    {
                        error.Mensaje += "Ingresar la contraseña ";
                    }
                    if (usuario.FechaNacimiento == "")
                    {
                        error.Mensaje += "Ingresar la fecha de nacimiento ";
                    }
                    if (usuario.Sexo == "")
                    {
                        error.Mensaje += "Ingresar el sexo ";
                    }
                    if (usuario.Telefono == "")
                    {
                        error.Mensaje += "Ingresar el telefono ";
                    }
                    if (usuario.Celular == "")
                    {
                        error.Mensaje += "Ingresar el celular ";
                    }
                    if (usuario.CURP == "")
                    {
                        error.Mensaje += "Ingresar el CURP ";
                    }
                    if (usuario.Rol.IdRol.ToString() == "")
                    {
                        error.Mensaje += "Ingresar el idRol ";
                    }
                    if (usuario.Direccion.Calle == "")
                    {
                        error.Mensaje += "Ingresar nombre de colonia ";
                    }
                    if (usuario.Direccion.NumeroInterior == "")
                    {
                        error.Mensaje += "Ingresar nombre de colonia ";
                    }
                    if (usuario.Direccion.NumeroExterior == "")
                    {
                        error.Mensaje += "Ingresar nombre de colonia ";
                    }
                    if (usuario.Direccion.Colonia.IdColonia.ToString() == "")
                    {
                        error.Mensaje += "Ingresar nombre de colonia ";
                    }

                    if (error.Mensaje != null)
                    {
                        result.Objects.Add(error);
                    }
                }
                result.Correct = true;
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }
    }//Usuario
}//NS