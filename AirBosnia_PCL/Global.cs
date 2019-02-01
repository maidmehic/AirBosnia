using AirBosnia_PCL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirBosnia_PCL
{
  public  class Global
    {
        public static Korisnik logiraniKorisnik{ get; set; }

        public const string RecommenderRoute = "api/Recommender";
        public const string LoginRoute = "api/Korisnik";
        public const string GradRoute = "api/Grad";
        public const string LetRoute = "api/Let";
        public const string SjedisteRoute = "api/Sjediste";
        public const string RezervacijaRoute = "api/Rezervacija";
        public const string PosebnaPonudaRoute = "api/PosebnaPonuda";
       
    }
}
