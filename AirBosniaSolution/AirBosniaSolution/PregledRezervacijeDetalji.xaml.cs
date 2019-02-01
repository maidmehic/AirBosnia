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
	public partial class PregledRezervacijeDetalji : ContentPage
	{

        private WebApiHelper rezervacijaServis = new WebApiHelper(Application.Current.Resources["APIAddress"].ToString(),Global.RezervacijaRoute);
        private WebApiHelper sjedisteServis = new WebApiHelper(Application.Current.Resources["APIAddress"].ToString(), Global.SjedisteRoute);

        Rezervacija Rezervacija;
        public PregledRezervacijeDetalji (int rezervacijaID)
		{
			InitializeComponent ();



            HttpResponseMessage response = rezervacijaServis.GetResponse(rezervacijaID.ToString());
            if (response.IsSuccessStatusCode)
            {
                var jsonObject = response.Content.ReadAsStringAsync();
                Rezervacija rezervacija = JsonConvert.DeserializeObject<Rezervacija>(jsonObject.Result);

                Rezervacija = rezervacija;

                if(rezervacija.Let.DatumVrijemePolaska> DateTime.Now)
                {
                    btnOtkazi.IsVisible = true;
                }
                else
                {
                    btnOtkazi.IsVisible = false;
                }

                txtDatum.Text = rezervacija.Let.DatumVrijemePolaska.ToString("d MMM, yyyy");
                txtRuta.Text = rezervacija.Let.Grad1.Naziv+" ("+ rezervacija.Let.Grad1.Oznaka+") -> "+ rezervacija.Let.Grad.Naziv + " (" + rezervacija.Let.Grad.Oznaka + ")";
                txtBrLeta.Text = rezervacija.Let.BrojLeta;
                txtVrPolaska.Text = rezervacija.Let.DatumVrijemePolaska.ToString("HH:mm");
                txtVrDolaska.Text = rezervacija.Let.DatumVrijemeDolaska.ToString("HH:mm");
                txtSjediste.Text = rezervacija.Sjediste.Oznaka;
                txtCijena.Text = rezervacija.CijenaKarte + " KM";

                txtImePrezime.Text = rezervacija.ImePutnika + " " + rezervacija.PrezimePutnika;
                txtDatumRodenja.Text = rezervacija.DatumRodjenjaPutnika.ToString("d MMM, yyyy");
                txtSpolTip.Text = rezervacija.Spol + " / " + rezervacija.TipPutnika;
                txtDokument.Text = rezervacija.TipDokumenta;
                txtBrDokumenta.Text = rezervacija.BrojDokumenta;
                txtDatumRezervacije.Text = rezervacija.DatumRezervacije.ToString("d MMM, yyyy HH:mm");
            }
            else
            {
                DisplayAlert("Greška", "Rezervacija se ne može pronaći", "OK");
            }
		}

        private void BtnOtkazi_Clicked(object sender, EventArgs e)
        {
            HttpResponseMessage response = sjedisteServis.GetResponse(Rezervacija.Sjediste.SjedisteID.ToString());
            if (response.IsSuccessStatusCode)
            {
                var jsonObject = response.Content.ReadAsStringAsync();
                Sjediste sjediste = JsonConvert.DeserializeObject<Sjediste>(jsonObject.Result);

                sjediste.isZauzeto = false;

                HttpResponseMessage oslobodiSjediste = sjedisteServis.PutResponse(sjediste.SjedisteID,sjediste);
                if (oslobodiSjediste.IsSuccessStatusCode)
                {
                    HttpResponseMessage izbrisiRezervaciju = rezervacijaServis.DeleteResponse(Rezervacija.RezervacijaID);
                    if (izbrisiRezervaciju.IsSuccessStatusCode)
                    {
                        DisplayAlert("Uspjeh", "Rezervacija uspješno otkazana", "OK");
                        Navigation.PushAsync(new PregledRezervacija());
                    }
                    else
                    {
                        DisplayAlert("Greška", "Rezervacija se ne može izbrisati", "OK");
                    }
                }
                else
                {
                    DisplayAlert("Greška", "Sjedište se ne može osloboditi", "OK");
                }
            }
            else
            {
                DisplayAlert("Greška", "Sjedište se ne može pronaći", "OK");
            }


        }
    }
}