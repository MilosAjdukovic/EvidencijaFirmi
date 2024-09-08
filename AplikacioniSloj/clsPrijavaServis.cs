using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlojPodataka.Interfejsi;
using DomenskiSloj;

namespace AplikacioniSloj
{
    public class clsPrijavaServis
    {
        private IPrijavaRepo _repo;
        private clsPoslovnaPravila _poslovnaPravila;

        //Konstruktor
        public clsPrijavaServis(IPrijavaRepo repo, clsPoslovnaPravila poslovnaPravila)
        {
            _repo = repo;
            _poslovnaPravila = poslovnaPravila;
        }

        public DataSet Prikazi()
        {
            return _repo.DajSvePrijave();
        }

        public DataSet Prikazi(string jmbgKorisnika)
        {
            return _repo.DajSvePrijavePoKorisniku(jmbgKorisnika);
        }
        public DataSet DajOblasti()
        {
            return _repo.DajSveOblasti();
        }

        public bool Dodaj(string jmbgKorisnika, string nazivFirme, int idOblasti)
        {
            if(!_poslovnaPravila.ProveraPrijave(jmbgKorisnika)) return false;
            if(!_poslovnaPravila.ProveraNaziva(nazivFirme)) return false;
            return _repo.NovaPrijava(jmbgKorisnika, nazivFirme, idOblasti);
        }

        public bool Odbij(int idPrijave)
        {
            return _repo.OdbijPrijavu(idPrijave);
        }

        public bool OdobriPrijavuIKreirajFirmu(int idPrijave)
        {
            string pibBroj = _poslovnaPravila.RacunanjePIBBroja();
            if(pibBroj == null) return false;
            return _repo.OdobriPrijavuIKreirajFirmu(idPrijave, pibBroj);
        }
    }
}
