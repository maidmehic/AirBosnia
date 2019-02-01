using AirBosnia_PCL;
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
	public partial class PreporuceniLetovi : ContentPage
	{
        private WebApiHelper recommenderServis = new WebApiHelper(Application.Current.Resources["APIAddress"].ToString(), Global.RecommenderRoute);

        Letovi_RezervacijaVM Letovi;

        public PreporuceniLetovi ()
		{
			InitializeComponent ();

            BindPreporuceniLetovi();
            BindKlase();

        }

        private void BindPreporuceniLetovi()
        {
            HttpResponseMessage response = recommenderServis.GetActionResponse("SearchRecommendedLet", Global.logiraniKorisnik.KorisnikID.ToString());

            if (response.IsSuccessStatusCode)
            {
                var jsonResult = response.Content.ReadAsStringAsync();
                Letovi_RezervacijaVM letovi = JsonConvert.DeserializeObject<Letovi_RezervacijaVM>(jsonResult.Result);
                letoviList.ItemsSource = letovi.podaci;
                Letovi = letovi;
                if (letovi.podaci.Count == 0)
                {
                    DisplayAlert("Info", "Trenutno nema preporučenih letova", "Nazad");
                }
            }
            else
            {
                DisplayAlert("Greška", "Nemoguće pristupiti preporučenim letovima", "Nazad");
            }
        }

        private void BindKlase()
        {
            List<string> klase = new List<string>();
            klase.Add("Ekonomska");
            klase.Add("Bussiness");

            klasaPicker.ItemsSource = klase;
            klasaPicker.SelectedIndex = 0;
        }

        private void KlasaPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (klasaPicker.SelectedItem != null)
            {
                if (klasaPicker.SelectedIndex == 0)
                {
                    foreach (Letovi_RezervacijaVM.Rows l in Letovi.podaci)
                    {
                        l.promjenaCijene = false;
                    }

                    letoviList.ItemsSource = Letovi.podaci.Where(x => x.BrojSlobodnihEkonomska > 0);

                }

                if (klasaPicker.SelectedIndex == 1)
                {
                    foreach (Letovi_RezervacijaVM.Rows l in Letovi.podaci)
                    {
                        l.promjenaCijene = true;
                    }
                    letoviList.ItemsSource = Letovi.podaci.Where(x => x.CijenaBussOdrasli != 0 && x.BrojSlobodnihBussiness > 0);
                }
            }
        }

        private void LetoviList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null)
            {
               this.Navigation.PushAsync(new PutnikDetalji(e.Item as Letovi_RezervacijaVM.Rows, klasaPicker.SelectedIndex, null, null));
            }
        }
    }
}