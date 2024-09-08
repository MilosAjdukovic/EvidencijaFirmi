using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlojPodataka.Interfejsi
{
    public interface IFirmaRepo
    {
        DataSet DajSveFirme();
        DataSet DajFirmuPoKorisniku(string jmbgKorisnika);
        bool ObrisiFirmu(int idFirme);
        bool IzmeniFirmu(int idFirme, string noviNaziv, int noviIdOblasti);
    }
}
