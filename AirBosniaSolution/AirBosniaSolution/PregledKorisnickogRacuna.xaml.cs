using AirBosnia_PCL;
using AirBosnia_PCL.Model;
using AirBosnia_PCL.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AirBosniaSolution
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PregledKorisnickogRacuna : ContentPage
	{
        private WebApiHelper korisnikServis = new WebApiHelper(Application.Current.Resources["APIAddress"].ToString(), Global.LoginRoute);

        bool urediLozinku = false;
        Korisnik korisnik;
        public PregledKorisnickogRacuna ()
		{
			InitializeComponent ();

            HttpResponseMessage response = korisnikServis.GetResponse(Global.logiraniKorisnik.KorisnikID.ToString());

            if (response.IsSuccessStatusCode)
            {
                var jsonObject = response.Content.ReadAsStringAsync();
                Korisnik k = JsonConvert.DeserializeObject<Korisnik>(jsonObject.Result);
                korisnik = k;

                imeInput.Text = k.Ime;
                prezimeInput.Text = k.Prezime;
                emailInput.Text = k.Email;
            }
            else
            {
                DisplayAlert("Greška", "Greška pri ponalaženju korisnika", "Nazad");
            }

        }

        private void BtnUrediLozinku_Clicked(object sender, EventArgs e)
        {
            if (urediLozinku)
            {
                urediLozinku = false;
                lozinkaInput.IsVisible = false;
                novaLozinkaInput.IsVisible = false;
                loz.IsVisible = false;
                loz1.IsVisible = false;
                btnUrediLozinku.Text = "Uredi lozinku";
            }
            else
            {
                urediLozinku = true;
                lozinkaInput.IsVisible = true;
                novaLozinkaInput.IsVisible = true;
                loz.IsVisible = true;
                loz1.IsVisible = true;
                btnUrediLozinku.Text = "Poništi uređivanje lozinke";
            }
            

        }

        private void BtnSpremi_Clicked(object sender, EventArgs e)
        {
            if (urediLozinku)
            {
                Regex regex = new Regex(@"^[a-žA-Ž]+$");
                Regex mailRegex = new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
                Match match1 = regex.Match(imeInput.Text ?? "");
                Match match2 = regex.Match(prezimeInput.Text ?? "");
                Match match3 = mailRegex.Match(emailInput.Text ?? "");
                HttpResponseMessage emailProvjera = korisnikServis.GetActionResponse("SearchByEmail", emailInput.Text, korisnik.KorisnikID.ToString());


                if (String.IsNullOrEmpty(imeInput.Text) || String.IsNullOrEmpty(prezimeInput.Text) || String.IsNullOrEmpty(emailInput.Text) || String.IsNullOrEmpty(lozinkaInput.Text) || String.IsNullOrEmpty(novaLozinkaInput.Text))
                    errorLabel.Text = "Sva polja su obavezna";
                else if (!match1.Success)
                    errorLabel.Text = "Ime se sastoji samo od slova";
                else if (imeInput.Text.Length <= 1)
                    errorLabel.Text = "Ime mora sadržavati više od 1 karaktera";
                else if (imeInput.Text.Length >= 50)
                    errorLabel.Text = "Ime mora sadržavati manje 50 karaktera";
                else if (!match2.Success)
                    errorLabel.Text = "Prezime se sastoji samo od slova";
                else if (prezimeInput.Text.Length <= 1)
                    errorLabel.Text = "Prezime mora sadržavati više od 1 karaktera";
                else if (prezimeInput.Text.Length >= 50)
                    errorLabel.Text = "Prezime mora sadržavati manje 50 karaktera";
                else if (!match3.Success)
                    errorLabel.Text = "Email adresa nije validna";
                else if (emailInput.Text.Length >= 50)
                    errorLabel.Text = "Email adresa mora sadržavati manje od 50 karaktera";
                else if (emailProvjera.IsSuccessStatusCode)
                    errorLabel.Text = "Email adresa je zauzeta";
                else if(korisnik.Lozinka!=lozinkaInput.Text)
                    errorLabel.Text = "Stara lozinka je netačna";
                else if (novaLozinkaInput.Text.Length <= 6)
                    errorLabel.Text = "Nova lozinka mora sadržavati više od 6 karaktera";
                else if (novaLozinkaInput.Text.Length >= 50)
                    errorLabel.Text = "Nova lozinka mora sadržavati manje od 50 karaktera";
                else
                {
                    korisnik.Email = emailInput.Text;
                    korisnik.Ime = imeInput.Text;
                    korisnik.Prezime = prezimeInput.Text;
                    korisnik.Lozinka = novaLozinkaInput.Text;

                    HttpResponseMessage respone = korisnikServis.PutResponse(korisnik.KorisnikID, korisnik);
                    if (respone.IsSuccessStatusCode)
                    {
                        DisplayAlert("Info", "Račun uspješno izmjenjen", "OK");
                        //Navigation.PopToRootAsync();
                    }
                    else
                    {
                        DisplayAlert("Greška", "Račun nije izmjenjen", "OK");
                    }
                }
            }
            else
            {
                Regex regex = new Regex(@"^[a-žA-Ž]+$");
                Regex mailRegex = new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
                Match match1 = regex.Match(imeInput.Text ?? "");
                Match match2 = regex.Match(prezimeInput.Text ?? "");
                Match match3 = mailRegex.Match(emailInput.Text ?? "");
                HttpResponseMessage emailProvjera = korisnikServis.GetActionResponse("SearchByEmail", emailInput.Text, korisnik.KorisnikID.ToString());


                if (String.IsNullOrEmpty(imeInput.Text) || String.IsNullOrEmpty(prezimeInput.Text) || String.IsNullOrEmpty(emailInput.Text))
                    errorLabel.Text = "Sva polja su obavezna";
                else if (!match1.Success)
                    errorLabel.Text = "Ime se sastoji samo od slova";
                else if (imeInput.Text.Length <= 1)
                    errorLabel.Text = "Ime mora sadržavati više od 1 karaktera";
                else if (imeInput.Text.Length >= 50)
                    errorLabel.Text = "Ime mora sadržavati manje 50 karaktera";
                else if (!match2.Success)
                    errorLabel.Text = "Prezime se sastoji samo od slova";
                else if (prezimeInput.Text.Length <= 1)
                    errorLabel.Text = "Prezime mora sadržavati više od 1 karaktera";
                else if (prezimeInput.Text.Length >= 50)
                    errorLabel.Text = "Prezime mora sadržavati manje 50 karaktera";
                else if (!match3.Success)
                    errorLabel.Text = "Email adresa nije validna";
                else if (emailInput.Text.Length >= 50)
                    errorLabel.Text = "Email adresa mora sadržavati manje od 50 karaktera";
                else if (emailProvjera.IsSuccessStatusCode)
                    errorLabel.Text = "Email adresa je zauzeta";
                else
                {
                    korisnik.Email = emailInput.Text;
                    korisnik.Ime = imeInput.Text;
                    korisnik.Prezime = prezimeInput.Text;

                    HttpResponseMessage respone = korisnikServis.PutResponse(korisnik.KorisnikID, korisnik);
                    if (respone.IsSuccessStatusCode)
                    {
                        DisplayAlert("Info", "Račun uspješno izmjenjen", "OK");
                        Navigation.PushModalAsync(new Navigacija());
                    }
                    else
                    {
                        DisplayAlert("Greška", "Račun nije izmjenjen", "OK");
                    }
                }
            }
           
        }
    }
}