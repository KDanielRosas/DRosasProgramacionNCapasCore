using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Usuario
    {
        //[Required]
        public int IdUsuario { get; set; }

        //[Required]
        public string UserName { get; set; }

        //[Required]
        //[RegularExpression("^[A-Za-z ]+$", ErrorMessage = "Solo se permite ingresar letras.")]
        public string Nombre { get; set; }

        //[Required]
        //[RegularExpression("^[A-Za-z ]+$", ErrorMessage = "Solo se permite ingresar letras.")]
        public string ApellidoPaterno { get; set; }

        //[Required]
        //[RegularExpression("^[A-Za-z ]+$", ErrorMessage = "Solo se permite ingresar letras.")]
        public string ApellidoMaterno { get; set; }

        //[Required]
        //[DataType(DataType.EmailAddress)]
        //[RegularExpression("^\\w+@[a-zA-Z_]+?\\.[a-zA-Z]{2,3}$", ErrorMessage = "Ingrese un e-mail válido.")]
        public string Email { get; set; }

        //[Required]
        //[RegularExpression("(?=.*\\d)(?=.*[A-Z])(?=.*[@#$%]).{0,10}", ErrorMessage = "La contraseña debe contener al menos 1 letra mayúscula, 1 caracter especial y maximo 10 caracteres.")]
        public string Password { get; set; }

        //[Required]
        public string FechaNacimiento { get; set; }

        //[Required]
        public string Sexo { get; set; }

        //[Required]
        //[RegularExpression("^[0-9]+$", ErrorMessage = "Debe de contener 10 digitos y ser solo numeros.")]
        //[StringLength(10)]
        public string Telefono { get; set; }

        //[Required]
        //[RegularExpression("^[0-9]+$", ErrorMessage = "Debe de contener 10 digitos y ser solo numeros.")]
        //[StringLength(10)]
        public string Celular { get; set; }

        //[Required]
        //[RegularExpression("^[A-Z0-9]+$", ErrorMessage = "Error en el formato del CURP, maximo 18 caracteres.")]
        //[StringLength(18)]
        public string CURP { get; set; }

        public string Imagen { get; set; }

        //[Required]
        public ML.Rol Rol { get; set; }

        //[Required]
        public ML.Direccion Direccion { get; set; }


        public List<object> Usuarios { get; set; }

        
        public string? NombreRol { get; set; }

        
        public int? IdDireccion { get; set; }

        public bool? Status { get; set; }

    }
}
