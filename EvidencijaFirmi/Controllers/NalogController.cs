using AplikacioniSloj;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SlojPodataka;
using System.Data;

public class NalogController : Controller
{
    private readonly clsKorisnikServis _korisnikServis;

    public NalogController(clsKorisnikServis korisnikServis)
    {
        _korisnikServis = korisnikServis;
    }

    [HttpGet]
    public IActionResult Registracija()
    {
        return View();
    }


    [HttpPost]
    public ActionResult Registracija(RegistracijaModel model)
    {
        if (ModelState.IsValid)
        {
            bool uspesnaRegistracija = _korisnikServis.Dodaj(new clsKorisnik
            {
                Jmbg = model.JMBG,
                Ime = model.Ime,
                Prezime = model.Prezime,
                Email = model.Email,
                Lozinka = model.Lozinka
            });

            if (uspesnaRegistracija)
            {
                // Ukoliko je registracija uspešna, preusmeri korisnika na odgovarajući view ili akciju
                return RedirectToAction("Prijava");
            }
            else
            {
                // Ukoliko registracija nije uspela, može se dodati odgovarajuća logika ili poruka
                ModelState.AddModelError(string.Empty, "Registracija nije uspešna. Pokušajte ponovo.");
            }
        }

        // Ako ModelState nije validan, vraća se isti view sa postojećim podacima
        return View(model);
    }

    [HttpGet]
    public ActionResult Prijava()
    {
        return View();
    }
    [HttpPost]
    public ActionResult Prijava(PrijavaModel model)
    {
        if (ModelState.IsValid)
        {
            // Pozovi metodu iz servisa koja proverava korisničke podatke
            var prijavljeniKorisnik = _korisnikServis.PronadjiPoEmail(model.Email);

            if (prijavljeniKorisnik != null)
            {
                // Ako je pronađen korisnik sa datom e-poštom, proveri lozinku
                if (prijavljeniKorisnik.Lozinka == model.Lozinka)
                {
                    // Lozinka je ispravna, postavi korisničke podatke u sesiju
                    HttpContext.Session.SetString("JMBG", prijavljeniKorisnik.Jmbg);
                    HttpContext.Session.SetString("Ime", prijavljeniKorisnik.Ime);
                    HttpContext.Session.SetString("Prezime", prijavljeniKorisnik.Prezime);
                    HttpContext.Session.SetString("Email", prijavljeniKorisnik.Email);
                    HttpContext.Session.SetString("Lozinka", prijavljeniKorisnik.Lozinka);
                    HttpContext.Session.SetString("TipKorisnika", prijavljeniKorisnik.TipKorisnika);

                    // Redirekcija na odgovarajući view u zavisnosti od toga kog tipa je korisnik
                    if (prijavljeniKorisnik.TipKorisnika == "admin")
                    {
                        return RedirectToAction("AdminPocetna", "Admin");
                    }
                    else if (prijavljeniKorisnik.TipKorisnika == "obican_korisnik")
                    {
                        return RedirectToAction("KorisnikPocetna", "Korisnik");
                    }
                }
                else
                {
                    // Pogrešna lozinka
                    ModelState.AddModelError(string.Empty, "Pogrešna lozinka");
                }
            }
            else
            {
                // Nema korisnika sa datom e-mailom
                ModelState.AddModelError(string.Empty, "Nema korisnika u bazi podataka sa navedenim e-mailom!");
            }
        }

        // Ako ModelState nije validan, vraća se isti view sa postojećim podacima
        return View(model);
    }

}
