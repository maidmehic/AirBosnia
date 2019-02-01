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
    public partial class PregledPosebnihPonuda : ContentPage
    {
        private WebApiHelper ponudaServis = new WebApiHelper(Application.Current.Resources["APIAddress"].ToString(), Global.PosebnaPonudaRoute);


        public PregledPosebnihPonuda()
        {
            InitializeComponent();
            BindPonude();
        }

        private void BindPonude()
        {
            HttpResponseMessage response = ponudaServis.GetActionResponse("SearchAktivnePonude",0.ToString());
            if (response.IsSuccessStatusCode)
            {
                var jsonObj = response.Content.ReadAsStringAsync();
                PretragaPosebnigPonudaVM ponude = JsonConvert.DeserializeObject<PretragaPosebnigPonudaVM>(jsonObj.Result);
                ponudeList.ItemsSource = ponude.podaci;
            }
        }

        private void PonudeList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item != null)
            {
                Navigation.PushAsync(new PregledPosPonudaDetalji(e.Item as PretragaPosebnigPonudaVM.Rows));
            }
        }
    }
}