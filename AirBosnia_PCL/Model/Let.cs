using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirBosnia_PCL.Model
{
   public class Let
    {
        public int LetID { get; set; }
        public int PolazisteID { get; set; }
        public int OdredisteID { get; set; }
        public int ZaposlenikID { get; set; }
        public string BrojLeta { get; set; }
        public int AvionID { get; set; }
        public System.DateTime DatumVrijemePolaska { get; set; }
        public System.DateTime DatumVrijemeDolaska { get; set; }
        public bool StatusLeta { get; set; }
        public Nullable<double> CijenaBussOdrasli { get; set; }
        public Nullable<double> CijenaBussDjeca { get; set; }
        public double CijenaEcoDjeca { get; set; }
        public double CijenaEcoOdrasli { get; set; }

        public virtual Grad Grad { get; set; }
        public virtual Grad Grad1 { get; set; }
    }
}
