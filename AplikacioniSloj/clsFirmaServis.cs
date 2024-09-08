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
    public class clsFirmaServis
    {
        private IFirmaRepo _repo;

        //Konstruktor
        public clsFirmaServis(IFirmaRepo repo)
        {
            _repo = repo;
        }

        public DataSet Prikazi()
        {
            return _repo.DajSveFirme();
        }

        public DataSet Prikazi(string jmbg)
        {
            return _repo.DajFirmuPoKorisniku(jmbg);
        }

        public bool Obrisi(int idFirme)
        {
            return _repo.ObrisiFirmu(idFirme);
        }

        public bool Izmeni(int idFirme, string noviNaziv, int noviIdOblasti)
        {
            return _repo.IzmeniFirmu(idFirme, noviNaziv, noviIdOblasti);
        }

    }
}
