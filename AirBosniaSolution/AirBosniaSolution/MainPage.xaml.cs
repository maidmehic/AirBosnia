using AirBosnia_PCL;
using AirBosnia_PCL.Model;
using AirBosnia_PCL.Util;
using AirBosnia_PCL.ViewModel;
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
	public partial class MainPage : ContentPage
	{
        private WebApiHelper lokacijaServis = new WebApiHelper(Application.Current.Resources["APIAddress"].ToString(), Global.GradRoute);
        private WebApiHelper letServis = new WebApiHelper(Application.Current.Resources["APIAddress"].ToString(), Global.LetRoute);

        int polazakIndex = -1;
        int destinacijaIndex = -1;
        public MainPage ()
		{
			InitializeComponent ();
           // NavigationPage.SetHasNavigationBar(this, false);

        }

        protected override void OnAppearing()
        {
          
            HttpResponseMessage response = lokacijaServis.GetActionResponse("GetPolazisteOdrediste", 0.ToString());
            if (response.IsSuccessStatusCode)
            {
                var jsonObj = response.Content.ReadAsStringAsync();
                List<Grad> vrste = JsonConvert.DeserializeObject<List<Grad>>(jsonObj.Result);
                PolazistePicker.ItemsSource = vrste.OrderBy(x=>x.Naziv).ToList();
                PolazistePicker.ItemDisplayBinding = new Binding("Prikazi");
                PolazistePicker.SelectedIndex = polazakIndex;
            }
            else
            {
                DisplayAlert("Greška", "Nemoguće pristupiti lokacijama", "OK");
            }

            HttpResponseMessage response1 = lokacijaServis.GetActionResponse("GetPolazisteOdrediste", 0.ToString());
            if (response1.IsSuccessStatusCode)
            {
                var jsonObj = response1.Content.ReadAsStringAsync();
                List<Grad> vrste = JsonConvert.DeserializeObject<List<Grad>>(jsonObj.Result);
                OdredistePicker.ItemsSource = vrste.OrderBy(x => x.Naziv).ToList();
                OdredistePicker.ItemDisplayBinding = new Binding("Prikazi");
                OdredistePicker.SelectedIndex = destinacijaIndex;
            }
            else
            {
                DisplayAlert("Greška", "Nemoguće pristupiti lokacijama", "OK");
            }
            base.OnAppearing();
        }

        private void SwPovratak_Toggled(object sender, ToggledEventArgs e)
        {
            if (swPovratak.IsToggled)
            {
                txtDatumPovratak.IsVisible = true;
                datumPovratkaLabel.IsVisible = true;
            }
            else
            {
                txtDatumPovratak.IsVisible = false;
                datumPovratkaLabel.IsVisible = false;
            }
        }

        private void BtnPretraga_Clicked(object sender, EventArgs e)
        {
            if (txtDatum.Date < DateTime.Now.Date)
            {
                DisplayAlert("Info", "Datum polaska ne smije biti manji od trenutnog vremena", "OK");
                return;
            }

            if (swPovratak.IsToggled)
            {
                if (txtDatumPovratak.Date < txtDatum.Date)
                {
                    DisplayAlert("Info", "Datum povratka ne smije biti manji od datuma polaska", "OK");
                    return;
                }
            }

            if (PolazistePicker.SelectedItem != null && OdredistePicker.SelectedItem != null)
            {
                

                int polazisteID = (PolazistePicker.SelectedItem as Grad).GradID;
                int odredisteID = (OdredistePicker.SelectedItem as Grad).GradID;
                string datumPolaska = txtDatum.Date.ToString().Replace("/", "-").Replace(":", ".");

                HttpResponseMessage response = letServis.GetActionResponse("SearchLetByPolazisteOdrediste", datumPolaska, polazisteID.ToString(), odredisteID.ToString());
                if (response.IsSuccessStatusCode)
                {
                    var jsonObj = response.Content.ReadAsStringAsync();
                    Letovi_RezervacijaVM letovi = JsonConvert.DeserializeObject<Letovi_RezervacijaVM>(jsonObj.Result);

                    if (letovi.podaci.Count == 0)
                    {
                        DisplayAlert("Info", "Nema dostupnih letova za polazak", "OK");
                    }
                    else
                    {
                        if (swPovratak.IsToggled)
                        {
                            string datumPovratka = txtDatumPovratak.Date.ToString().Replace("/", "-").Replace(":", ".");
                            HttpResponseMessage responsePovratak = letServis.GetActionResponse("SearchLetByPolazisteOdrediste", datumPovratka, odredisteID.ToString(), polazisteID.ToString());

                            if (responsePovratak.IsSuccessStatusCode)
                            {
                                var jsonObjPovratak = responsePovratak.Content.ReadAsStringAsync();
                                Letovi_RezervacijaVM letoviPovratak = JsonConvert.DeserializeObject<Letovi_RezervacijaVM>(jsonObjPovratak.Result);

                                if (letoviPovratak.podaci.Count == 0)
                                {
                                    DisplayAlert("Info", "Nema dostupnih letova za povratak", "OK");
                                }
                                else
                                {
                                    Navigation.PushAsync(new PretragaLetova(letovi, letoviPovratak));
                                }

                            }
                            else
                            {
                                DisplayAlert("Greška", "Nemoguće pristupiti letovima za povratak", "OK");
                            }
                        }
                        else
                        {
                            Navigation.PushAsync(new PretragaLetova(letovi, null));
                        }
                    }
                }
                else
                {
                    DisplayAlert("Greška", "Nemoguće pristupiti letovima za polazak", "OK");
                }
            }
            else
            {
                DisplayAlert("Greška", "Potrebno je odabrati polazište i destinaciju", "Nazad");
            }
        }

        private void OdredistePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (OdredistePicker.SelectedIndex != -1)
                destinacijaIndex = OdredistePicker.SelectedIndex;
        }

        private void PolazistePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PolazistePicker.SelectedIndex != -1)
                polazakIndex = PolazistePicker.SelectedIndex;
        }
    }
}