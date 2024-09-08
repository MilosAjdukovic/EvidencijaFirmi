using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlojPodataka
{
    // Class: Firma - Prati podatke o firmama.

    //// Responsibility:
    //- Drži podatke o firmama (IDFirme, PIBBroj, Naziv, IDOblasti, JMBGKorisnika, IDStatusFirme).
    //- Omogućava pristup i modifikaciju podataka o firmama.

    //// Collaboration:
    //- Sa klasom Korisnik (veza sa JMBGKorisnika).

    [Table("FIRMA")]
    public class clsFirma
    {

        //Polja
        [Key]
        private int _idFirme;

        [Required]
        private string _pibBroj;

        [Required]
        private string _naziv;

        [Required]
        private int _idOblasti;

        [Required]
        private string _jmbgKorisnika;

        [Required]
        private int _idStatusFirme;

        //Property
        public int IdFirme { get => _idFirme; set => _idFirme = value; }
        public string PibBroj { get => _pibBroj; set => _pibBroj = value; }
        public string Naziv { get => _naziv; set => _naziv = value; }
        public int IdOblasti { get => _idOblasti; set => _idOblasti = value; }
        public string JmbgKorisnika { get => _jmbgKorisnika; set => _jmbgKorisnika = value; }
        public int IdStatusFirme { get => _idStatusFirme; set => _idStatusFirme = value; }
    }
}
