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
    public partial class Navigacija : MasterDetailPage
    {
        public Navigacija()
        {
            InitializeComponent();
            MasterPage.ListView.ItemSelected += ListView_ItemSelected;
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as NavigacijaMenuItem;
            if (item == null)
                return;
            
            if(item.Title== "  Odjava")
            {
                Application.Current.MainPage = new NavigationPage(new Login());
                return;
            }

            var page = (Page)Activator.CreateInstance(item.TargetType);
            page.Title = item.Title;

            Detail = new NavigationPage(page);
            IsPresented = false;

            MasterPage.ListView.SelectedItem = null;
        }
    }
}