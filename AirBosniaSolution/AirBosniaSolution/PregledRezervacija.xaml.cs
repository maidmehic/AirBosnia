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
	public partial class PregledRezervacija : ContentPage
	{
        private WebApiHelper rezervacijaServis = new WebApiHelper(Application.Current.Resources["APIAddress"].ToString(), Global.RezervacijaRoute);

        public PregledRezervacija ()
		{
			InitializeComponent ();
            BindRezervacije();
		}

        private void BindRezervacije()
        {
            HttpResponseMessage response = rezervacijaServis.GetActionResponse("SearchLetByLogiraniKorisnik", Global.logiraniKorisnik.KorisnikID.ToString());
            if (response.IsSuccessStatusCode)
            {
                var jsonObj = response.Content.ReadAsStringAsync();
                Korisnikove_RezervacijeVM letovi = JsonConvert.DeserializeObject<Korisnikove_RezervacijeVM>(jsonObj.Result);
                letoviList.ItemsSource = letovi.podaci;
            }
        }

        private void LetoviList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null)
            {
                Navigation.PushAsync(new PregledRezervacijeDetalji((e.Item as Korisnikove_RezervacijeVM.Rows).RezervacijaID));
            }
        }
    }
}