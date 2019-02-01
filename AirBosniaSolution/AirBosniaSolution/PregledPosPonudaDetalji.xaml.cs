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
	public partial class PregledPosPonudaDetalji : ContentPage
	{
        PretragaPosebnigPonudaVM.Rows Ponuda;
        public PregledPosPonudaDetalji (PretragaPosebnigPonudaVM.Rows p)
		{
			InitializeComponent ();
            Ponuda = p;
            BindForm();
		}

        private void BindForm()
        {
            txtDestinacija.Text = Ponuda.Destinacija;
            txtPolazak.Text = Ponuda.Polazak;
            txtDatumPolaska.Text = Ponuda.DatumPolaska;
            txtDatumPovratka.Text = Ponuda.DatumPovratka;
            txtCijena.Text = Ponuda.Cijena.ToString()+" KM";
            txtCijenaCjeca.Text = Ponuda.CijenaDjeca.ToString()+" KM";
            txtSmjestak.Text = Ponuda.Smjestaj;
            txtNapomena.Text = Ponuda.Napomena;
        }

        private void BtnRezervacija_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RezervacijaPosebnePonude(Ponuda.LetPolazakID, Ponuda.LetDolazakID, Ponuda.Cijena,Ponuda.CijenaDjeca));
        }
    }
}