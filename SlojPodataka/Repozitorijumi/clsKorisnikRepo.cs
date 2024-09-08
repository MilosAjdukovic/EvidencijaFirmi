using SlojPodataka;
using SlojPodataka.Interfejsi;
using System.Data;
using System.Data.SqlClient;

namespace SlojPodataka.Repozitorijumi
{
    public class clsKorisnikRepo : IKorisnikRepo
    {
        //Polje za konekciju
        private string _stringKonekcije;

        //Konstruktor
        //Dobije se string konekcije pri pozivanju
        public clsKorisnikRepo(string stringKonekcije)
        {
            _stringKonekcije = stringKonekcije;
        }

        //Izlistavanje svih korisnike i daje njihov jmbg, ime, prezime, tip, e-mail,
        //lozinku preko join-a i daje opis kog je tipa korisnik umesto idTipaKorisnika koja je numericka vrednost
        public DataSet DajSveKorisnike()
        {
            DataSet dsPodaci = new DataSet();

            SqlConnection Veza = new SqlConnection(_stringKonekcije);
            Veza.Open();
            SqlCommand Komanda = new SqlCommand("DajSveKorisnike", Veza);
            Komanda.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = Komanda;
            da.Fill(dsPodaci);
            Veza.Close();
            Veza.Dispose();

            return dsPodaci;
        }


        //Filtriraju se korisnici po prezimenu sa LIKE
        public DataSet DajKorisnikaPoPrezimenu(string Prezime)
        {

            DataSet dsPodaci = new DataSet();

            SqlConnection Veza = new SqlConnection(_stringKonekcije);
            Veza.Open();
            SqlCommand Komanda = new SqlCommand("DajKorisnikaPoPrezimenu", Veza);
            Komanda.CommandType = CommandType.StoredProcedure;
            Komanda.Parameters.Add("@Prezime", SqlDbType.NVarChar).Value = Prezime;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = Komanda;
            da.Fill(dsPodaci);
            Veza.Close();
            Veza.Dispose();

            return dsPodaci;
        }

        public DataSet DajKorisnikaPoJMBG(string jmbg)
        {

            DataSet dsPodaci = new DataSet();

            SqlConnection Veza = new SqlConnection(_stringKonekcije);
            Veza.Open();
            SqlCommand Komanda = new SqlCommand("DajKorisnikaPoJMBG", Veza);
            Komanda.CommandType = CommandType.StoredProcedure;
            Komanda.Parameters.Add("@JMBG", SqlDbType.NVarChar).Value = jmbg;
            SqlDataAdapter da = new SqlDataAdapter();
            da.SelectCommand = Komanda;
            da.Fill(dsPodaci);
            Veza.Close();
            Veza.Dispose();

            return dsPodaci;
        }

        public bool NoviKorisnik(clsKorisnik objNoviKorisnik)
        {
            //Promenljiva koja služi za proveru uspesnosti unosa 
            int proveraUnosa = 0;

            SqlConnection Veza = new SqlConnection(_stringKonekcije);
            Veza.Open();
            SqlCommand Komanda = new SqlCommand("NoviKorisnik", Veza);
            Komanda.CommandType = CommandType.StoredProcedure;
            Komanda.Parameters.Add("@JMBG", SqlDbType.NVarChar).Value = objNoviKorisnik.Jmbg;
            Komanda.Parameters.Add("@Ime", SqlDbType.NVarChar).Value = objNoviKorisnik.Ime;
            Komanda.Parameters.Add("@Prezime", SqlDbType.NVarChar).Value = objNoviKorisnik.Prezime;
            Komanda.Parameters.Add("@Email", SqlDbType.NVarChar).Value = objNoviKorisnik.Email;
            Komanda.Parameters.Add("@Lozinka", SqlDbType.NVarChar).Value = objNoviKorisnik.Lozinka;

            proveraUnosa = Komanda.ExecuteNonQuery();
            Veza.Close();
            Veza.Dispose();

            //Vraća se true ako je uspesno
            return (proveraUnosa > 0);
        }

        public bool ObrisiKorisnika(string JMBG)
        {
            int proveraUnosa = 0;

            SqlConnection Veza = new SqlConnection(_stringKonekcije);
            Veza.Open();

            SqlCommand Komanda = new SqlCommand("ObrisiKorisnika", Veza);
            Komanda.CommandType = CommandType.StoredProcedure;
            Komanda.Parameters.Add("@JMBG", SqlDbType.NVarChar).Value = JMBG;

            proveraUnosa = Komanda.ExecuteNonQuery();
            Veza.Close();
            Veza.Dispose();

            return (proveraUnosa > 0);

        }

      
        public bool IzmeniKorisnika(string StariJMBG, clsKorisnik objNoviKorisnik)
        {
            int proveraUnosa = 0;

            SqlConnection Veza = new SqlConnection(_stringKonekcije);
            Veza.Open();

            SqlCommand Komanda = new SqlCommand("IzmeniKorisnika", Veza);
            Komanda.CommandType = CommandType.StoredProcedure;
            Komanda.Parameters.Add("@StariJMBG", SqlDbType.NVarChar).Value = StariJMBG;
            Komanda.Parameters.Add("@JMBG", SqlDbType.NVarChar).Value = objNoviKorisnik.Jmbg;
            Komanda.Parameters.Add("@Ime", SqlDbType.NVarChar).Value = objNoviKorisnik.Ime;
            Komanda.Parameters.Add("@Prezime", SqlDbType.NVarChar).Value = objNoviKorisnik.Prezime;
            Komanda.Parameters.Add("@Email", SqlDbType.NVarChar).Value = objNoviKorisnik.Email;
            Komanda.Parameters.Add("@Lozinka", SqlDbType.NVarChar).Value = objNoviKorisnik.Lozinka;

            proveraUnosa = Komanda.ExecuteNonQuery();
            Veza.Close();
            Veza.Dispose();

            return (proveraUnosa > 0);
        }

        public clsKorisnik PronadjiPoEmail(string email)
        {
            using (SqlConnection Veza = new SqlConnection(_stringKonekcije))
            {
                
                Veza.Open();
                SqlCommand Komanda = new SqlCommand("PronadjiKorisnikaPoEmailu", Veza);
                Komanda.CommandType = CommandType.StoredProcedure;
                Komanda.Parameters.Add("@Email", SqlDbType.NVarChar).Value = email;

                using (SqlDataReader Reader = Komanda.ExecuteReader())
                {
                    if (Reader.Read())
                    {
                        return MapirajRedUObjekat(Reader);
                    }
                    else
                    {
                        return null; // Nema pronađenog korisnika sa datim email-om
                    }
                }
            }

        }

        private clsKorisnik MapirajRedUObjekat(SqlDataReader reader)
        {
            return new clsKorisnik
            {
                Jmbg = reader["JMBG"].ToString(),
                Ime = reader["Ime"].ToString(),
                Prezime = reader["Prezime"].ToString(),
                Email = reader["Email"].ToString(),
                Lozinka = reader["Lozinka"].ToString(),
                TipKorisnika = reader["TipKorisnika"].ToString()
            };
        }

    }
}