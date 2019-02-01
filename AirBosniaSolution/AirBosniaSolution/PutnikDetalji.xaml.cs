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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AirBosniaSolution
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PutnikDetalji : ContentPage
	{

        private WebApiHelper sjedisteServis = new WebApiHelper(Application.Current.Resources["APIAddress"].ToString(), Global.SjedisteRoute);
        private WebApiHelper rezervacijaServis = new WebApiHelper(Application.Current.Resources["APIAddress"].ToString(),Global.RezervacijaRoute);

        Letovi_RezervacijaVM.Rows OdabraniLetPolazak;
        int OdabranaKlasaPolazak;

        Letovi_RezervacijaVM.Rows OdabraniLetPovratak=null;
        int? OdabranaKlasaPovratak = null;

        public PutnikDetalji (Letovi_RezervacijaVM.Rows odabraniLetPolazak, int odabranaKlasaPolazak, Letovi_RezervacijaVM.Rows odabraniLetPovratak, int ? odabranaKlasaPovratak)
		{
			InitializeComponent ();

            OdabraniLetPolazak = odabraniLetPolazak;
            OdabranaKlasaPolazak = odabranaKlasaPolazak;

            OdabraniLetPovratak = odabraniLetPovratak;
            OdabranaKlasaPovratak = odabranaKlasaPovratak;

            if(OdabranaKlasaPovratak != null && OdabraniLetPovratak != null)
            {
                LabelPovratakSjediste.IsVisible = true;
                pickerSjedistePovratak.IsVisible = true;
            }

            BindPickers();
		}

        private void BindPickers()
        {
            List<string> spol = new List<string>();
            spol.Add("Odaberite spol");
            spol.Add("Muški");
            spol.Add("Ženski");
            pickerSpol.ItemsSource = spol;
            pickerSpol.SelectedIndex = 0;

            List<string> tipPutnika = new List<string>();
            tipPutnika.Add("Odrasli");
            tipPutnika.Add("Djeca");
            pickerTipPutnika.ItemsSource = tipPutnika;
            pickerTipPutnika.SelectedIndex = 0;

            List<string> tipDokumenta = new List<string>();
            tipDokumenta.Add("Odaberite tip");
            tipDokumenta.Add("Pasoš");
            tipDokumenta.Add("Lična karta");
            tipDokumenta.Add("Viza za osobno putovanje");
            tipDokumenta.Add("Viza za poslovno putovanje");
            pickerTipDokumenta.ItemsSource = tipDokumenta;
            pickerTipDokumenta.SelectedIndex = 0;


            HttpResponseMessage response = sjedisteServis.GetActionResponse("SearchSlobodnaSjedista", OdabraniLetPolazak.LetID.ToString(), OdabranaKlasaPolazak.ToString());
            if (response.IsSuccessStatusCode)
            {
                var jsonObject = response.Content.ReadAsStringAsync();
                Sjedista_IndexVM sjedista = JsonConvert.DeserializeObject<Sjedista_IndexVM>(jsonObject.Result);
                pickerSjediste.ItemsSource = sjedista.sjedista;
                pickerSjediste.ItemDisplayBinding = new Binding("OznakaSaProzorom");
            }
            else
            {
                DisplayAlert("Greška", "Neka greška", "Nazad");
            }

            if (OdabranaKlasaPovratak != null)
            {
                HttpResponseMessage responseSjedistaPovratak = sjedisteServis.GetActionResponse("SearchSlobodnaSjedista", OdabraniLetPovratak.LetID.ToString(), OdabranaKlasaPovratak.ToString());
                if (responseSjedistaPovratak.IsSuccessStatusCode)
                {
                    var jsonObject = responseSjedistaPovratak.Content.ReadAsStringAsync();
                    Sjedista_IndexVM sjedista = JsonConvert.DeserializeObject<Sjedista_IndexVM>(jsonObject.Result);
                    pickerSjedistePovratak.ItemsSource = sjedista.sjedista;
                    pickerSjedistePovratak.ItemDisplayBinding = new Binding("OznakaSaProzorom");
                }
                else
                {
                    DisplayAlert("Greška", "Neka greška", "Nazad");
                }
            }

        }

        private void BtnProcesiraj_Clicked(object sender, EventArgs e)
        {

            Regex regex = new Regex(@"^[a-žA-Ž]+$");
            Match match1 = regex.Match(txtIme.Text ?? "");
            Match match2 = regex.Match(txtPrezime.Text ?? "");
            int BrojGodina = DateTime.Now.Date.Year - txtDatumRodenja.Date.Year;

            if (String.IsNullOrEmpty(txtIme.Text) || String.IsNullOrEmpty(txtPrezime.Text) || pickerSpol.SelectedIndex==0
               || pickerTipPutnika.SelectedIndex == -1 || pickerTipDokumenta.SelectedIndex == 0 || String.IsNullOrEmpty(txtBrojDokumenta.Text))
                errorLabel.Text = "Sva polja označena * su obavezna";
            else if(!match1.Success)
                errorLabel.Text = "Ime se sastoji samo od slova";
            else if (txtIme.Text.Length <= 1)
                errorLabel.Text = "Ime mora sadržavati više od 1 karaktera";
            else if (txtIme.Text.Length >= 50)
                errorLabel.Text = "Ime mora sadržavati manje 50 karaktera";
            else if (!match2.Success)
                errorLabel.Text = "Prezime se sastoji samo od slova";
            else if (txtPrezime.Text.Length <= 1)
                errorLabel.Text = "Prezime mora sadržavati više od 1 karaktera";
            else if (txtPrezime.Text.Length >= 50)
                errorLabel.Text = "Prezime mora sadržavati manje 50 karaktera";
            else if (txtBrojDokumenta.Text.Length <= 1)
                errorLabel.Text = "Broj dokumenta mora sadržavati više od 1 karaktera";
            else if (txtBrojDokumenta.Text.Length >= 100)
                errorLabel.Text = "Broj dokumenta mora sadržavati manje 100 karaktera";
            else if(txtDatumRodenja.Date>DateTime.Now.Date)
                errorLabel.Text = "Datum rođenja putnika ne može biti veći od trenutnog datuma";
            else if(pickerTipPutnika.SelectedIndex==1 && BrojGodina>12)
                errorLabel.Text = "Odabrani tip putnika je netačan. Putnik ima više od 12 godina";
            else
            {
                Rezervacija nova = new Rezervacija();

                nova.KorisnikID = Global.logiraniKorisnik.KorisnikID;
                nova.LetPolazakID = OdabraniLetPolazak.LetID;

                if (pickerSjediste.SelectedItem != null)
                {
                    nova.SjedistePolazakID = (pickerSjediste.SelectedItem as Sjedista_IndexVM.Rows).SjedisteID;
                }
                else
                {
                    pickerSjediste.SelectedIndex = 0;
                    nova.SjedistePolazakID = (pickerSjediste.SelectedItem as Sjedista_IndexVM.Rows).SjedisteID;
                }

                nova.ImePutnika = txtIme.Text;
                nova.PrezimePutnika = txtPrezime.Text;
                nova.DatumRodjenjaPutnika = txtDatumRodenja.Date;
                nova.DatumRezervacije = DateTime.Now;
                nova.Spol = pickerSpol.SelectedItem.ToString();
                nova.TipPutnika = pickerTipPutnika.SelectedItem.ToString();
                nova.TipDokumenta = pickerTipDokumenta.SelectedItem.ToString();
                nova.BrojDokumenta = txtBrojDokumenta.Text;



                if (OdabranaKlasaPolazak == 0)
                {
                    if (pickerTipPutnika.SelectedIndex == 0)
                    {
                        nova.CijenaKarte = OdabraniLetPolazak.CijenaEcoOdrasli;
                    }
                    if (pickerTipPutnika.SelectedIndex == 1)
                    {
                        nova.CijenaKarte = OdabraniLetPolazak.CijenaEcoDjeca;
                    }
                }

                if (OdabranaKlasaPolazak == 1)
                {
                    if (pickerTipPutnika.SelectedIndex == 0)
                    {
                        nova.CijenaKarte = OdabraniLetPolazak.CijenaBussOdrasli ?? 0;
                    }
                    if (pickerTipPutnika.SelectedIndex == 1)
                    {
                        nova.CijenaKarte = OdabraniLetPolazak.CijenaBussDjeca ?? 0;
                    }
                }

                HttpResponseMessage response = rezervacijaServis.PostResponse(nova);
                if (response.IsSuccessStatusCode)
                {

                    HttpResponseMessage getSjediste = sjedisteServis.GetResponse(nova.SjedistePolazakID.ToString());
                    if (getSjediste.IsSuccessStatusCode)
                    {
                        var jsonObject = getSjediste.Content.ReadAsStringAsync();
                        Sjediste odabranoSjediste = JsonConvert.DeserializeObject<Sjediste>(jsonObject.Result);
                        odabranoSjediste.isZauzeto = true;
                        HttpResponseMessage izmjenaSjedista = sjedisteServis.PutResponse(nova.SjedistePolazakID, odabranoSjediste);

                        if (izmjenaSjedista.IsSuccessStatusCode)
                        {
                            DisplayAlert("Uspjeh", "Rezervacija uspješno spremljena", "OK");

                            if (OdabranaKlasaPovratak == null && OdabraniLetPovratak == null)
                            {
                                ClearForm();

                                var rezID = response.Content.ReadAsStringAsync();
                                int rezervacijID = JsonConvert.DeserializeObject<int>(rezID.Result);
                                  Navigation.PushAsync(new PregledRezervacijeDetalji(rezervacijID));
                            }
                        }
                        else
                        {
                            DisplayAlert("Greška", "Sjedište se ne može izmjeniti", "OK");
                        }
                    }
                    else
                    {
                        DisplayAlert("Greška", "Sjedište se ne može naći", "OK");
                    }
                }
                else
                {
                    DisplayAlert("Greška", "Rezervacija nije spremljena", "OK");
                }


                //Spremanje povratne karte
                if (OdabranaKlasaPovratak != null && OdabraniLetPovratak != null)
                {
                    Rezervacija povratna = new Rezervacija();

                    povratna.KorisnikID = Global.logiraniKorisnik.KorisnikID;
                    povratna.LetPolazakID = OdabraniLetPovratak.LetID;

                    if (pickerSjedistePovratak.SelectedItem != null)
                    {
                        povratna.SjedistePolazakID = (pickerSjedistePovratak.SelectedItem as Sjedista_IndexVM.Rows).SjedisteID;
                    }
                    else
                    {
                        pickerSjedistePovratak.SelectedIndex = 0;
                        povratna.SjedistePolazakID = (pickerSjedistePovratak.SelectedItem as Sjedista_IndexVM.Rows).SjedisteID;
                    }

                    povratna.ImePutnika = txtIme.Text;
                    povratna.PrezimePutnika = txtPrezime.Text;
                    povratna.DatumRodjenjaPutnika = txtDatumRodenja.Date;
                    povratna.DatumRezervacije = DateTime.Now;
                    povratna.Spol = pickerSpol.SelectedItem.ToString();
                    povratna.TipPutnika = pickerTipPutnika.SelectedItem.ToString();
                    povratna.TipDokumenta = pickerTipDokumenta.SelectedItem.ToString();
                    povratna.BrojDokumenta = txtBrojDokumenta.Text;

                    if (OdabranaKlasaPovratak == 0)
                    {
                        if (pickerTipPutnika.SelectedIndex == 0)
                        {
                            povratna.CijenaKarte = OdabraniLetPovratak.CijenaEcoOdrasli;
                        }
                        if (pickerTipPutnika.SelectedIndex == 1)
                        {
                            povratna.CijenaKarte = OdabraniLetPovratak.CijenaEcoDjeca;
                        }
                    }

                    if (OdabranaKlasaPovratak == 1)
                    {
                        if (pickerTipPutnika.SelectedIndex == 0)
                        {
                            povratna.CijenaKarte = OdabraniLetPovratak.CijenaBussOdrasli ?? 0;
                        }
                        if (pickerTipPutnika.SelectedIndex == 1)
                        {
                            povratna.CijenaKarte = OdabraniLetPovratak.CijenaBussDjeca ?? 0;
                        }
                    }

                    HttpResponseMessage responsePovratna = rezervacijaServis.PostResponse(povratna);
                    if (responsePovratna.IsSuccessStatusCode)
                    {

                        HttpResponseMessage getSjediste = sjedisteServis.GetResponse(povratna.SjedistePolazakID.ToString());
                        if (getSjediste.IsSuccessStatusCode)
                        {
                            var jsonObject = getSjediste.Content.ReadAsStringAsync();
                            Sjediste odabranoSjediste = JsonConvert.DeserializeObject<Sjediste>(jsonObject.Result);
                            odabranoSjediste.isZauzeto = true;
                            HttpResponseMessage izmjenaSjedista = sjedisteServis.PutResponse(povratna.SjedistePolazakID, odabranoSjediste);

                            if (izmjenaSjedista.IsSuccessStatusCode)
                            {

                                ClearForm();

                                var rezID = responsePovratna.Content.ReadAsStringAsync();
                                int rezervacijID = JsonConvert.DeserializeObject<int>(rezID.Result);

                                Navigation.PushAsync(new PregledRezervacijeDetalji(rezervacijID));

                            }
                            else
                            {
                                DisplayAlert("Greška", "Sjedište se ne može izmjeniti", "OK");
                            }
                        }
                        else
                        {
                            DisplayAlert("Greška", "Sjedište se ne može naći", "OK");
                        }
                    }
                    else
                    {
                        DisplayAlert("Greška", "Rezervacija nije spremljena", "OK");
                    }

                }

               
            }

            

        }

        private void ClearForm()
        {
            txtIme.Text = "";
            txtPrezime.Text = "";
            txtDatumRodenja.Date = DateTime.Now;
            txtBrojDokumenta.Text = "";
            pickerSpol.SelectedIndex = 0;
            pickerTipPutnika.SelectedIndex = 0;
            pickerTipDokumenta.SelectedIndex = 0;
            pickerSjediste.SelectedIndex = -1;
        }
    }
}