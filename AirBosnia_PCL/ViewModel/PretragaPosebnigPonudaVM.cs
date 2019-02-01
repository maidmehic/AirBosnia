using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirBosnia_PCL.ViewModel
{
   public class PretragaPosebnigPonudaVM
    {
        public class Rows
        {
            public int PonudaID { get; set; }
            public string Destinacija { get; set; }
            public string Polazak { get; set; }
            public string Smjestaj { get; set; }
            public string Napomena { get; set; }
            public string DatumPovratka { get; set; }
            public string DatumDaniNocenja { get; set; }
            public double CijenaDjeca { get; set; }
            public string DatumPolaska { get; set; }
            public double Cijena { get; set; }

            public int LetPolazakID { get; set; }
            public int LetDolazakID { get; set; }
        }
        public List<Rows> podaci { get; set; }
    }
}
