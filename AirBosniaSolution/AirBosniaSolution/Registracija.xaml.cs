using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using AirBosnia_PCL.Util;
using System.Net.Http;
using Newtonsoft.Json;
using AirBosnia_PCL.Model;
using System.Text.RegularExpressions;
using AirBosnia_PCL;

namespace AirBosniaSolution
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Registracija : ContentPage
	{
        private WebApiHelper korisnikServis = new WebApiHelper(Application.Current.Resources["APIAddress"].ToString(), Global.LoginRoute);

		public Registracija ()
		{
			InitializeComponent ();
		}

        private  void BtnRegistracija_Clicked(object sender, EventArgs e)
        {
            Regex regex = new Regex(@"^[a-žA-Ž]+$");
            Regex mailRegex = new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            Match match1 = regex.Match(imeInput.Text??"");
            Match match2 = regex.Match(prezimeInput.Text??"");
            Match match3 = mailRegex.Match(emailInput.Text ?? "");
            HttpResponseMessage emailProvjera = korisnikServis.GetActionResponse("SearchByEmail", emailInput.Text, 0.ToString());


            if (String.IsNullOrEmpty(imeInput.Text) || String.IsNullOrEmpty(prezimeInput.Text) || String.IsNullOrEmpty(emailInput.Text) || String.IsNullOrEmpty(lozinkaInput.Text))
                errorLabel.Text = "Sva polja su obavezna";
            else if (!match1.Success)
                errorLabel.Text = "Ime se sastoji samo od slova";
            else if (imeInput.Text.Length <= 1)
                errorLabel.Text = "Ime mora sadržavati više od 1 karaktera";
            else if(imeInput.Text.Length >= 50)
                errorLabel.Text = "Ime mora sadržavati manje 50 karaktera";
            else if (!match2.Success)
                errorLabel.Text = "Prezime se sastoji samo od slova";
            else if (prezimeInput.Text.Length <= 1)
                errorLabel.Text = "Prezime mora sadržavati više od 1 karaktera";
            else if (prezimeInput.Text.Length >= 50)
                errorLabel.Text = "Prezime mora sadržavati manje 50 karaktera";
            else if (lozinkaInput.Text.Length <= 6)
                errorLabel.Text = "Lozinka mora sadržavati više od 6 karaktera";
            else if (lozinkaInput.Text.Length >= 50)
                errorLabel.Text = "Lozinka mora sadržavati manje od 50 karaktera";
            else if (!match3.Success)
                errorLabel.Text = "Email adresa nije validna";
            else if (emailInput.Text.Length >= 50)
                errorLabel.Text = "Email adresa mora sadržavati manje od 50 karaktera";
            else if (emailProvjera.IsSuccessStatusCode)
                errorLabel.Text = "Email adresa je zauzeta";
            else
            {
                Korisnik k = new Korisnik();
                k.Ime = imeInput.Text;
                k.Prezime = prezimeInput.Text;
                k.Email = emailInput.Text;
                k.Lozinka = lozinkaInput.Text;

                HttpResponseMessage respone = korisnikServis.PostResponse(k);
                if (respone.IsSuccessStatusCode)
                {
                    DisplayAlert("Info", "Registracija uspješno obavljena", "OK");
                    Navigation.PushAsync(new Login());
                }
                else
                {
                    DisplayAlert("Info", "Registracija nije obavljena", "OK");
                }
            }

        }
    }
}