using AirBosnia_PCL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AirBosniaSolution
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NavigacijaMaster : ContentPage
    {
        public ListView ListView;

        public NavigacijaMaster()
        {
            InitializeComponent();

            BindingContext = new NavigacijaMasterViewModel();
            ListView = MenuItemsListView;
            labelNavigacija.Text = Global.logiraniKorisnik.Ime + " " + Global.logiraniKorisnik.Prezime;
            labelNavigacija1.Text = Global.logiraniKorisnik.Email;
        }

        class NavigacijaMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<NavigacijaMenuItem> MenuItems { get; set; }
            
            public NavigacijaMasterViewModel()
            {
                MenuItems = new ObservableCollection<NavigacijaMenuItem>(new[]
                {
                    new NavigacijaMenuItem { Title = "  Pretraga letova", imageSource="search.png",TargetType=typeof(MainPage) },
                    new NavigacijaMenuItem { Title = "  Rezervacije", imageSource="plane_tickets.png",TargetType=typeof(PregledRezervacija) },
                    new NavigacijaMenuItem { Title = "  Preporučeni letovi", imageSource="insignia.png",TargetType=typeof(PreporuceniLetovi) },
                    new NavigacijaMenuItem { Title = "  Posebne ponude", imageSource="sunbed.png",TargetType=typeof(PregledPosebnihPonuda) },
                    new NavigacijaMenuItem { Title = "  Uredi račun", imageSource="user.png",TargetType=typeof(PregledKorisnickogRacuna) },
                    new NavigacijaMenuItem { Title = "  Odjava", imageSource="logout.png",TargetType=typeof(Login) },
                    
                });
            }
            
            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}