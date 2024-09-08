using System.ComponentModel.DataAnnotations;

public class PrijavaModel
{
    [Required(ErrorMessage = "Unesite e-mail.")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Unesite lozinku.")]
    [DataType(DataType.Password)]
    public string Lozinka { get; set; }
}
