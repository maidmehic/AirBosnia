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
	public partial class PretragaLetova : ContentPage
	{
        Letovi_RezervacijaVM Letovi;
        Letovi_RezervacijaVM LetoviPovratak = null;
		public PretragaLetova (Letovi_RezervacijaVM letovi, Letovi_RezervacijaVM letoviPovratak)
		{
			InitializeComponent ();

            Letovi = letovi;
            LetoviPovratak = letoviPovratak;

            letoviList.ItemsSource = letovi.podaci;
            BindKlase();
        }

        private void BindKlase()
        {
            List<string> klase = new List<string>();
            klase.Add("Ekonomska");
            klase.Add("Bussiness");

            klasaPicker.ItemsSource = klase;
            klasaPicker.SelectedIndex = 0;
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            //Navigation.PushAsync(new MainPage(PolazisteIndex, DestinacijaIndex, odabraniDatum));
        }

        private void LetoviList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null)
            {
                if (LetoviPovratak != null)
                {
                    this.Navigation.PushAsync(new PretragaLetovaPovratak(e.Item as Letovi_RezervacijaVM.Rows, klasaPicker.SelectedIndex,LetoviPovratak));
                }
                else
                {
                    this.Navigation.PushAsync(new PutnikDetalji(e.Item as Letovi_RezervacijaVM.Rows, klasaPicker.SelectedIndex,null,null));
                }
            }
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

                    letoviList.ItemsSource = Letovi.podaci.Where(x=>x.BrojSlobodnihEkonomska>0);

                }

                if (klasaPicker.SelectedIndex == 1)
                {
                    foreach (Letovi_RezervacijaVM.Rows l in Letovi.podaci)
                    {
                        l.promjenaCijene = true;
                    }
                    letoviList.ItemsSource = Letovi.podaci.Where(x => x.CijenaBussOdrasli != 0 && x.BrojSlobodnihBussiness>0);
                }
            }
        }
    }
}