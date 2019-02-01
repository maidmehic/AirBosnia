using AirBosnia_PCL;
using AirBosnia_PCL.Model;
using AirBosnia_PCL.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AirBosniaSolution
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Login : ContentPage
	{

        private WebApiHelper korisnikServis = new WebApiHelper(Application.Current.Resources["APIAddress"].ToString(), Global.LoginRoute);

        public Login ()
		{
            //Application.Current.MainPage = new NavigationPage(new Login());
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
            Global.logiraniKorisnik = null;
        }

        private void BtnPriava_Clicked(object sender, EventArgs e)
        {
            if(String.IsNullOrEmpty(txtEmail.Text) || String.IsNullOrEmpty(txtLozinka.Text))
            {
                errorLabel.Text = "Email i lozinka su obavezno polje";
            }
            else
            {

                errorLabel.Text =null;

                HttpResponseMessage response = korisnikServis.GetActionResponse("SearchKorisnikByPrijava", txtEmail.Text, txtLozinka.Text, 0.ToString());

                if (response.IsSuccessStatusCode)
                {
                    var jsonResult = response.Content.ReadAsStringAsync();
                    Korisnik k = JsonConvert.DeserializeObject<Korisnik>(jsonResult.Result);
                    Global.logiraniKorisnik = k;
                    Navigation.PushModalAsync(new Navigacija());
                }
                else
                {
                    DisplayAlert("Greška", "Pogrešan email ili lozinka", "Nazad");
                }
            }

            
        }

        private async void BtnRegistracija_Clicked(object sender, EventArgs e)
        {
          await Navigation.PushAsync(new Registracija());
        }
    }
}