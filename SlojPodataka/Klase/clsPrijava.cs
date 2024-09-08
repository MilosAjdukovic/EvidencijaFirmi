using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlojPodataka
{
    // Class: Prijava - Prati podatke o prijavama za registraciju firme.

    //// Responsibility:
    //- Drži podatke o prijavama (IDPrijave, JMBGKorisnika, IDOblasti, StatusPrijaveID, NazivFirme).
    //- Omogućava pristup i modifikaciju podataka o prijavama.

    //// Collaboration:
    //- Sa klasom Korisnik (veza sa JMBGKorisnika u tabeli Prijava).
    //- Sa klasom Firma (povezana sa prijavom prilikom kreiranja firme).

    [Table("PRIJAVA")]
    public class clsPrijava
    {
        //Polja
        [Key]
        private int _idPrijave;

        [Required]
        [RegularExpression(@"^[0 - 9] +$")]
        private string _jmbgKorisnika;

        [Required]
        private string _imeFirme;

        [Required]
        private int _idOblasti;

        [Required]
        private int _statusPrijave;

        //Property
        public int IdPrijave { get => _idPrijave; set => _idPrijave = value; }
        public string JmbgKorisnika { get => _jmbgKorisnika; set => _jmbgKorisnika = value; }
        public int StatusPrijave { get => _statusPrijave; set => _statusPrijave = value; }
        public string ImeFirme { get => _imeFirme; set => _imeFirme = value; }
        public int IdOblasti { get => _idOblasti; set => _idOblasti = value; }
    }
}
