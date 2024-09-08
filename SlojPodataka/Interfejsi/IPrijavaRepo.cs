using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SlojPodataka.Interfejsi
{
    public interface IPrijavaRepo
    {
        bool NovaPrijava(string jmbgKorisnika, string nazivFirme, int idOblasti);
        DataSet DajSvePrijave();
        DataSet DajSvePrijavePoKorisniku(string jmbgKorisnika);
        bool OdobriPrijavuIKreirajFirmu(int idPrijave, string pibBroj);
        bool OdbijPrijavu(int idPrijave);
        DataSet DajSveOblasti();
    }
}
