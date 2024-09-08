
using SlojPodataka;
using SlojPodataka.Interfejsi;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Serialization.Json;
using System.Xml.Linq;

namespace DomenskiSloj
{
    public class clsPoslovnaPravila
    {
        private IPrijavaRepo _repoPrijava;
        private IFirmaRepo _repoFirma;
        private IKorisnikRepo _repoKorisnik;

        //Konstruktor
        //Dobija se string konekcije pri pozivanju
        public clsPoslovnaPravila(IPrijavaRepo repoPrijava, IFirmaRepo repoFirma, IKorisnikRepo repoKorisnik)
        {
            _repoPrijava = repoPrijava;
            _repoFirma = repoFirma;
            _repoKorisnik = repoKorisnik;
        }

        public bool ProveraPrijave(string jmbg)
        {
            bool proveraUspesnosti = false;

            DataSet dsPodaci = _repoPrijava.DajSvePrijave();

            if (dsPodaci != null)
            {
                var rezultat = from DataRow row in dsPodaci.Tables[0].AsEnumerable()
                               where row.Field<string>("JMBGKorisnika") == jmbg
                               select row;

                if (rezultat != null)
                {
                    DataRow[] nizRedova = rezultat.ToArray();

                    if (nizRedova.Length == 0)
                    {
                        proveraUspesnosti = true;
                    }
                    else
                    {
                        var prijaveNaCekanju = from DataRow red in nizRedova
                                               where red.Field<int>("StatusPrijaveID") == 1
                                               select red;

                        if (prijaveNaCekanju.Count() == 0)
                        {
                            proveraUspesnosti = true;
                        }
                    }
                }
                else
                {
                    proveraUspesnosti = true;
                }
            }

            return proveraUspesnosti;
        }


        public bool ProveraNaziva(string nazivFirme)
        {
            bool proveraUspesnosti = false;

            DataSet dsPodaci = _repoFirma.DajSveFirme();

            if (dsPodaci == null || dsPodaci.Tables.Count == 0 || dsPodaci.Tables[0].Rows.Count == 0)
            {
                proveraUspesnosti = true;
                return proveraUspesnosti;
            }
            var rezultat = from DataRow row in dsPodaci.Tables[0].AsEnumerable()
                           where row.Field<string>("NazivFirme") == nazivFirme
                           select row;

            if (!rezultat.Any())
            {
                proveraUspesnosti = true;
                return proveraUspesnosti;
            }

            return proveraUspesnosti;
        }

        public string RacunanjePIBBroja()
        {
            DataSet dsPodaci = _repoFirma.DajSveFirme();

            List<string> pibBrojevi;

            if (dsPodaci == null || dsPodaci.Tables.Count == 0 || dsPodaci.Tables[0].Rows.Count == 0)
            {
                pibBrojevi = new List<string>();
            }
            else
            {
                pibBrojevi = new List<string>();

                var rezultat = from DataRow row in dsPodaci.Tables[0].AsEnumerable()
                               select row.Field<string>("PIBBroj");

                pibBrojevi.AddRange(rezultat);
            }

            var random = new Random();
            string pibBroj;
            do
            {
                pibBroj = random.Next(100000001, 1000000000).ToString("D9");
            } while (pibBrojevi.Contains(pibBroj));

            return pibBroj;
        }


    }
}