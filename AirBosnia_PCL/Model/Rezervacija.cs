using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirBosnia_PCL.Model
{
   public class Rezervacija
    {
        public int RezervacijaID { get; set; }
        public int KorisnikID { get; set; }
        public int LetPolazakID { get; set; }
        public int SjedistePolazakID { get; set; }
        public string ImePutnika { get; set; }
        public string PrezimePutnika { get; set; }
        public DateTime DatumRodjenjaPutnika { get; set; }
        public string Spol { get; set; }
        public string TipPutnika { get; set; }
        public string TipDokumenta { get; set; }
        public string BrojDokumenta { get; set; }
        public DateTime DatumRezervacije { get; set; }
        public double CijenaKarte { get; set; }
        

        public virtual Korisnik Korisnik { get; set; }
        public virtual Let Let { get; set; }
        public virtual Sjediste Sjediste { get; set; }
    }
}
