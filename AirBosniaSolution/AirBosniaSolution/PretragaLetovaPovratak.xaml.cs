using AirBosnia_PCL.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AirBosniaSolution
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PretragaLetovaPovratak : ContentPage
	{
        Letovi_RezervacijaVM  PovratniLetovi;

        Letovi_RezervacijaVM.Rows OdabraniLetPolazak;
        int OdabranaKlasaPolazak;

        public PretragaLetovaPovratak (Letovi_RezervacijaVM.Rows odabraniletPolazak, int odabranaKlasaPolazak, Letovi_RezervacijaVM letoviPovratak)
		{
			InitializeComponent ();
            PovratniLetovi = letoviPovratak;
            letoviPovratakList.ItemsSource = PovratniLetovi.podaci;

            OdabraniLetPolazak = odabraniletPolazak;
            OdabranaKlasaPolazak = odabranaKlasaPolazak;

            BindKlase();
        }

        private void BindKlase()
        {
            List<string> klase = new List<string>();
            klase.Add("Ekonomska");
            klase.Add("Bussiness");

            klasaPickerPovratak.ItemsSource = klase;
            klasaPickerPovratak.SelectedIndex = 0;
        }

        private void KlasaPickerPovratak_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (klasaPickerPovratak.SelectedItem != null)
            {
                if (klasaPickerPovratak.SelectedIndex == 0)
                {
                    foreach (Letovi_RezervacijaVM.Rows l in PovratniLetovi.podaci)
                    {
                        l.promjenaCijene = false;
                    }

                    letoviPovratakList.ItemsSource = PovratniLetovi.podaci.Where(x => x.BrojSlobodnihEkonomska > 0);

                }

                if (klasaPickerPovratak.SelectedIndex == 1)
                {
                    foreach (Letovi_RezervacijaVM.Rows l in PovratniLetovi.podaci)
                    {
                        l.promjenaCijene = true;
                    }
                    letoviPovratakList.ItemsSource = PovratniLetovi.podaci.Where(x => x.CijenaBussOdrasli != 0 && x.BrojSlobodnihBussiness > 0);
                }
            }
        }

        private void LetoviPovratakList_ItemTapped(object sender, ItemTappedEventArgs e)
        {

            if (e.Item != null)
            {
                this.Navigation.PushAsync(new PutnikDetalji(OdabraniLetPolazak, OdabranaKlasaPolazak, e.Item as Letovi_RezervacijaVM.Rows, klasaPickerPovratak.SelectedIndex));
            }

        }
    }
}