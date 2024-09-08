using System.ComponentModel.DataAnnotations;

public class RegistracijaModel
{
    [Required(ErrorMessage = "JMBG je obavezan.")]
    [StringLength(13, ErrorMessage = "JMBG ne sme biti duži od 13 cifara.")]
    [RegularExpression(@"^[0-9]{13}$")]
    public string JMBG { get; set; }

    [Required(ErrorMessage = "Ime je obavezno.")]
    [StringLength(40, ErrorMessage = "Ime ne sme biti duže od 40 karaktera.")]
    public string Ime { get; set; }

    [Required(ErrorMessage = "Prezime je obavezno.")]
    [StringLength(40, ErrorMessage = "PRezime ne sme biti duže od 40 karaktera.")]
    public string Prezime { get; set; }


    [Required(ErrorMessage = "E-mail adresa je obavezna.")]
    [EmailAddress(ErrorMessage = "Neispravna e-mail adresa.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Lozinka je obavezna.")]
    [DataType(DataType.Password)]
    public string Lozinka { get; set; }
}
