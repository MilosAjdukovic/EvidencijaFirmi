using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SlojPodataka
{
    // Class: Korisnik - Prati podatke o korisnicima.

    //// Responsibility:
    //- Drži podatke o korisnicima (JMBG, Ime, Prezime, Email, Lozinka, TipKorisnika).
    //- Omogućava pristup i modifikaciju podataka o korisnicima.

    //// Collaboration:
    //- Sa klasom Firma (veza sa JMBGKorisnika u tabeli Firma).
    //- Sa klasom Prijava (veza sa JMBGKorisnika u tabeli Prijava).

    [Table("KORISNIK")]
    public class clsKorisnik
    {
        //Polja
        [Key]
        [RegularExpression(@"^[0-9]{13}$")]
        private string _jmbg;
        
        [Required]
        [StringLength(20)]
        private string _ime;
        
        [Required]
        [StringLength(40)]
        private string _prezime;

        [Required]
        [StringLength(20)]
        [RegularExpression(@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Z|a-z]{2,}$")]
        private string _email;

        [Required]
        [StringLength(20)]
        private string _lozinka;

        [Required]
        [StringLength(20)]
        private string _tipKorisnika;

        //Property
        public string Jmbg { get => _jmbg; set => _jmbg = value; }
        public string Ime { get => _ime; set => _ime = value; }
        public string Prezime { get => _prezime; set => _prezime = value; }
        public string Email { get => _email; set => _email = value; }
        public string Lozinka { get => _lozinka; set => _lozinka = value; }
        public string TipKorisnika { get => _tipKorisnika; set => _tipKorisnika = value; }
    }
}