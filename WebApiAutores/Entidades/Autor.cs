using System.ComponentModel.DataAnnotations;
using WebApiAutores.Validaciones;

namespace WebApiAutores.Entidades;

public class Autor : IValidatableObject
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El campo {0} es requerido")]
    [StringLength(maximumLength: 120, ErrorMessage = "El campo {0} no debe de tener más de {1} carácteres")]
    [PrimeraLetraMayuscula]
    public string Nombre { get; set; }

    //[Range(18, 120)]
    //[NotMapped]
    //public int Edad { get; set; }
    //[CreditCard]
    //[NotMapped] ------> p' tener props que no van a estar en las tablas
    //public string TarjetaDeCredito { get; set; }
    //[Url]
    //[NotMapped]
    //public string URL { get; set; }

    //public int Menor { get; set; }
    //public int Mayor { get; set; }

    public List<Libro> Libros { get; set; }


    // validacion a nivel modelo
    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (!string.IsNullOrEmpty(Nombre))
        {
            var primeraLetra = Nombre[0].ToString();

            if (primeraLetra != primeraLetra.ToUpper())
            {
                yield return new ValidationResult("La primera letra debe ser mayúscula",
                        new string[] { nameof(Nombre) }); // prop q tiene el error
            }
        }

        //if (Menor > Mayor)
        //{
        //    yield return new ValidationResult("Este valor no puede ser más grande que el campo Mayor",
        //        new string[] { nameof(Menor) });
        //}
    }
}
