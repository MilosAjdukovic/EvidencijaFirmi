using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System;
using AplikacioniSloj;
using SlojPodataka;
using System.Data;

namespace PrezentacioniSloj.Controllers
{
    public class KorisnikController : Controller
    {

        private readonly clsKorisnikServis _korisnikServis;
        private readonly clsFirmaServis _firmaServis;
        private readonly clsPrijavaServis _prijavaServis;

        public KorisnikController(clsKorisnikServis korisnikServis, clsFirmaServis firmaServis, clsPrijavaServis prijavaServis)
        {
            _korisnikServis = korisnikServis;
            _firmaServis = firmaServis;
            _prijavaServis = prijavaServis;
        }
        public IActionResult KorisnikPocetna()
        {
            return View();
        }

        public IActionResult KorisnikProfil()
        {
            // Dobijanje svih podataka iz sesije
            var jmbg = HttpContext.Session.GetString("JMBG");
            var ime = HttpContext.Session.GetString("Ime");
            var prezime = HttpContext.Session.GetString("Prezime");
            var lozinka = HttpContext.Session.GetString("Lozinka");
            var email = HttpContext.Session.GetString("Email");

            // Kreiranje modela sa podacima na osnovu sesije
            var model = new RegistracijaModel
            {
                JMBG = jmbg,
                Ime = ime,
                Prezime = prezime,
                Email = email,
                Lozinka = lozinka

            };

            return View(model);
        }

        public IActionResult KorisnikPrijava()
        {
            // Pretpostavljamo da dobijamo JMBG korisnika na osnovu sesije
            string jmbgKorisnika = HttpContext.Session.GetString("JMBG");

            // Dobijamo sve oblasti
            DataSet sveOblasti = _prijavaServis.DajOblasti();

            // Dobijamo sve prijave po korisniku
            DataSet prijave = _prijavaServis.Prikazi(jmbgKorisnika);

            DataSet dataSet = new DataSet();

            // Kopiramo i preimenujemo tabele pre nego što ih ubacimo u novi DataSet
            if (sveOblasti.Tables.Count > 0)
            {
                DataTable sveOblastiTable = sveOblasti.Tables[0].Copy();
                sveOblastiTable.TableName = "SveOblasti";
                dataSet.Tables.Add(sveOblastiTable);
            }

            if (prijave.Tables.Count > 0)
            {
                DataTable prijaveTable = prijave.Tables[0].Copy();
                prijaveTable.TableName = "Prijave";
                dataSet.Tables.Add(prijaveTable);
            }

            return View(dataSet);
        }

        [HttpPost]
        public IActionResult PrijaviFirmu(int idOblasti, string nazivFirme)
        {
            
            string jmbgKorisnika = HttpContext.Session.GetString("JMBG");

            if (_prijavaServis.Dodaj(jmbgKorisnika, nazivFirme, idOblasti))
            {
                TempData["SuccessMessage"] = "Uspešna prijava!";
                return RedirectToAction("KorisnikPocetna");
            }
            TempData["SuccessMessage"] = "Neuspešna prijava!";
            return RedirectToAction("KorisnikPrijava");
        }

        [HttpPost]
        public IActionResult IzmeniPodatke(RegistracijaModel model, string action)
        {

            if (action == "izmeni")
            {
                // Dobavi JMBG korisnika iz sesije
                var jmbgIzSesije = HttpContext.Session.GetString("JMBG");

                if (!string.IsNullOrEmpty(jmbgIzSesije))
                {
                    clsKorisnik korisnik = new clsKorisnik();
                    korisnik.Jmbg = model.JMBG;
                    korisnik.Ime = model.Ime;
                    korisnik.Prezime = model.Prezime;
                    korisnik.Email = model.Email;
                    korisnik.Lozinka = model.Lozinka;

                    if (_korisnikServis.Izmeni(jmbgIzSesije, korisnik))
                    {
                        TempData["SuccessMessage"] = "Uspešno izvršeno!";
                        return RedirectToAction("KorisnikPocetna");
                    }
                        
                    return View();
                }
                return View();

            }

            else if (action == "obrisi")
            {   
                var jmbg = HttpContext.Session.GetString("JMBG");

                if (!string.IsNullOrEmpty(jmbg))
                {
                    if (_korisnikServis.Obrisi(jmbg))
                        return RedirectToAction("Pocetna", "Home");
                    return View();
                }
                return View();
            }
            return View();
        }


        public IActionResult KorisnikStampa()
        {
            var jmbg = HttpContext.Session.GetString("JMBG");
            if (!string.IsNullOrEmpty(jmbg))
            {
                DataSet dataSet = _firmaServis.Prikazi(jmbg);
                if (dataSet != null)
                    return View("KorisnikStampa", dataSet);
                else return RedirectToAction("KorisnikPocetna");
            }
            return RedirectToAction("KorisnikPocetna");
        }
    }
}
