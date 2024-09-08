using AplikacioniSloj;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SlojPodataka;
using System;
using System.Data;

namespace PrezentacioniSloj.Controllers
{
    public class AdminController : Controller
    {
        private readonly clsKorisnikServis _korisnikServis;
        private readonly clsFirmaServis _firmaServis;
        private readonly clsPrijavaServis _prijavaServis;

        public AdminController(clsKorisnikServis korisnikServis, clsFirmaServis firmaServis, clsPrijavaServis prijavaServis)
        {
            _korisnikServis = korisnikServis;
            _firmaServis = firmaServis;
            _prijavaServis = prijavaServis;
        }

        public IActionResult AdminPocetna()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AdminPregledKorisnika(string prezime)
        {
            DataSet rezultat;

            if (!string.IsNullOrEmpty(prezime))
            {
                rezultat = _korisnikServis.Prikazi(prezime);
            }
            else
            {
                rezultat = _korisnikServis.Prikazi();
            }

            return View(rezultat);
        }

        public IActionResult AdminPregledPrijava()
        {
            DataSet prijave = _prijavaServis.Prikazi();

            return View(prijave);
        }

        [HttpPost]
        public IActionResult UpravljajPrijavama(int idPrijave, string action)
        {
            if (action == "odobri")
            {
                _prijavaServis.OdobriPrijavuIKreirajFirmu(idPrijave);
            }
            else if (action == "odbij")
            {
                _prijavaServis.Odbij(idPrijave);
            }

            return RedirectToAction("AdminPregledPrijava");
        }


        public IActionResult AdminPregledKorisnikaDetalji()
        {
            return View();
        }

        [HttpPost]
        public IActionResult IzmeniKorisnika(string? email, string? jmbg, string? action)
        {
            
                if (action == "izmeni")
                {
                    clsKorisnik korisnik = _korisnikServis.PronadjiPoEmail(email);
                    return View("AdminPregledKorisnikaDetalji", korisnik);
                }
                if (action == "obrisi")
                {
                    _korisnikServis.Obrisi(jmbg);
                }
            return RedirectToAction("AdminPregledKorisnika");
        }

        [HttpPost]
        public IActionResult IzmeniPodatke(string action, clsKorisnik model, string StariJMBG)
        {
            if (action == "izmeni")
            {
                string stariJMBG = StariJMBG;

                _korisnikServis.Izmeni(stariJMBG, model);

                TempData["SuccessMessage"] = "Uspešno izvršeno!";
                return RedirectToAction("AdminPocetna");
            }

            return View();
        }


        public IActionResult AdminFirmeStampa()
        {
            DataSet dataSet = _firmaServis.Prikazi();
                if (dataSet != null)
                    return View("AdminFirmeStampa", dataSet);
                else return RedirectToAction("AdminPocetna");
        }

        public IActionResult AdminPregledFirmi(string jmbgKorisnika)
        {
            DataSet dataSet;

            // Preuzmi sve korisnike iz baze podataka
            DataSet korisniciDataSet = _korisnikServis.Prikazi();

            if (korisniciDataSet != null && korisniciDataSet.Tables.Count > 0)
            {
                ViewBag.SviKorisnici = korisniciDataSet.Tables[0];
            }
            else
            {
                ViewBag.SviKorisnici = null; 
            }

            // Ukoliko nije odabran filter po korisniku, prikaži sve projekte sa korisnicima
            if (string.IsNullOrEmpty(jmbgKorisnika))
            {
                dataSet = _firmaServis.Prikazi();
            }
            else
            {
                // U drugom slučaju, filtriraj projekte prema odabranom JMBG-u korisnika
                dataSet = _firmaServis.Prikazi(jmbgKorisnika); // Metoda je u servisu za filtriranje po JMBG-u
            }

            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                return View("AdminPregledFirmi", dataSet);
            }
            else
            {
                return RedirectToAction("AdminPocetna");
            }
        }

        [HttpPost]
        public IActionResult AdminIzmeniFirmu(int idFirme, string action)
        {
            if (action == "izmeni")
            {
                var dataSetFirme = _firmaServis.Prikazi(); // Dobij sve firme
                var firmaTable = dataSetFirme.Tables[0]; // Pronađi odgovarajući DataTable za firme

                var firmaRow = firmaTable.AsEnumerable().FirstOrDefault(row => row.Field<int>("IdFirme") == idFirme);

                if (firmaRow == null)
                {
                    return NotFound(); // Ako firma nije pronađena, vrati NotFound
                }

                var firma = new SlojPodataka.clsFirma
                {
                    IdFirme = firmaRow.Field<int>("IdFirme"),
                    Naziv = firmaRow.Field<string>("NazivFirme"),
                    IdOblasti = firmaRow.Field<int>("IdOblasti")
                };

                // Dohvati sve oblasti
                var dataSetOblasti = _prijavaServis.DajOblasti();
                var oblastiTable = dataSetOblasti.Tables[0]; // Pronađi odgovarajući DataTable za oblasti

                // Pretvori DataTable u listu SelectListItem
                var oblastiList = oblastiTable.AsEnumerable().Select(row => new SelectListItem
                {
                    Value = row.Field<int>("IdOblasti").ToString(),
                    Text = row.Field<string>("ImeOblasti")
                }).ToList();

                ViewBag.OblastiList = oblastiList;

                return View("AdminIzmeniFirmu", firma);
            }

            if (action == "obrisi")
            {
                _firmaServis.Obrisi(idFirme);
            }

            return RedirectToAction("AdminPregledFirmi");
        }

        [HttpPost]
        public IActionResult IzmeniPodatkeZaFirmu(string action, string naziv, int IdOblasti, int IdFirme)
        {
            if (action == "izmeni")
            {
                // Pretpostavljam da `_firmaServis.Izmeni` zahteva nazivFirme
                _firmaServis.Izmeni(IdFirme, naziv, IdOblasti);

                TempData["SuccessMessage"] = "Uspešno izvršeno!";
                return RedirectToAction("AdminPocetna");
            }

            return View();
        }




    }
}
