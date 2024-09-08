using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlojPodataka.Interfejsi;

namespace SlojPodataka.Repozitorijumi
{
    public class clsPrijavaRepo : IPrijavaRepo
    {
        //Polje za konekciju
        private string _stringKonekcije;

        //Konstruktor
        //Dobije se string konekcije pri pozivanju
        public clsPrijavaRepo(string stringKonekcije)
        {
            _stringKonekcije = stringKonekcije;
        }

        public bool NovaPrijava(string jmbgKorisnika, string nazivFirme, int idOblasti)
        {
            //Promenljiva koja služi za proveru uspesnosti unosa 
            int proveraUnosa = 0;

            SqlConnection Veza = new SqlConnection(_stringKonekcije);
            Veza.Open();
            SqlCommand Komanda = new SqlCommand("NovaPrijava", Veza);
            Komanda.CommandType = CommandType.StoredProcedure;
            Komanda.Parameters.Add("@JMBGKorisnika", SqlDbType.NVarChar).Value = jmbgKorisnika;
            Komanda.Parameters.Add("@NazivFirme", SqlDbType.NVarChar).Value = nazivFirme;
            Komanda.Parameters.Add("@IDOblasti", SqlDbType.NVarChar).Value = idOblasti;

            proveraUnosa = Komanda.ExecuteNonQuery();
            Veza.Close();
            Veza.Dispose();

            //Vraća se true ako je uspesno
            return (proveraUnosa > 0);
        }

        public DataSet DajSvePrijave()
        {
            DataSet dsPodaci = new DataSet();

            SqlConnection Veza = new SqlConnection(_stringKonekcije);
            Veza.Open();
            SqlCommand Komanda = new SqlCommand("DajSvePrijave", Veza);
            Komanda.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = Komanda;
            da.Fill(dsPodaci);
            Veza.Close();
            Veza.Dispose();

            return dsPodaci;
        }

        public DataSet DajSvePrijavePoKorisniku(string jmbgKorisnika)
        {
            DataSet dsPodaci = new DataSet();

            SqlConnection Veza = new SqlConnection(_stringKonekcije);
            Veza.Open();
            SqlCommand Komanda = new SqlCommand("DajSvePrijavePoKorisniku", Veza);
            Komanda.CommandType = CommandType.StoredProcedure;
            Komanda.Parameters.Add("@JMBGKorisnika", SqlDbType.NVarChar).Value = jmbgKorisnika;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = Komanda;
            da.Fill(dsPodaci);
            Veza.Close();
            Veza.Dispose();

            return dsPodaci;
        }

        public bool OdbijPrijavu(int idPrijave)
        {
            int proveraUnosa = 0;

            SqlConnection Veza = new SqlConnection(_stringKonekcije);
            Veza.Open();
            SqlCommand Komanda = new SqlCommand("OdbijPrijavu", Veza);
            Komanda.CommandType = CommandType.StoredProcedure;
            Komanda.Parameters.Add("@IDPrijave", SqlDbType.Int).Value = idPrijave;

            proveraUnosa = Komanda.ExecuteNonQuery();
            Veza.Close();
            Veza.Dispose();

            //Vraća true ako je uspesno
            return (proveraUnosa > 0);
        }

        public bool OdobriPrijavuIKreirajFirmu(int idPrijave, string pibBroj)
        {
            //Promenljiva koja služi za proveru uspesnosti unosa
            int proveraUnosa = 0;

            SqlConnection Veza = new SqlConnection(_stringKonekcije);
            Veza.Open();
            SqlCommand Komanda = new SqlCommand("OdobriPrijavuIKreirajFirmu", Veza);
            Komanda.CommandType = CommandType.StoredProcedure;
            Komanda.Parameters.Add("@IDPrijave", SqlDbType.Int).Value = idPrijave;
            Komanda.Parameters.Add("@PIBBroj", SqlDbType.NVarChar).Value = pibBroj;

            proveraUnosa = Komanda.ExecuteNonQuery();
            Veza.Close();
            Veza.Dispose();

            //Vraća se true ako je uspesno
            return (proveraUnosa > 0);
        }

        public DataSet DajSveOblasti()
        {
            DataSet dsPodaci = new DataSet();

            SqlConnection Veza = new SqlConnection(_stringKonekcije);
            Veza.Open();
            SqlCommand Komanda = new SqlCommand("DajSveOblasti", Veza);
            Komanda.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = Komanda;
            da.Fill(dsPodaci);
            Veza.Close();
            Veza.Dispose();

            return dsPodaci;
        }
    }
}
