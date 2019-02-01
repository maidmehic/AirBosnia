using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirBosnia_PCL.Model
{
   public class Sjediste
    {
        public int SjedisteID { get; set; }
        public string Oznaka { get; set; }
        public int LetID { get; set; }
        public bool isZauzeto { get; set; }
        public bool isBussiness { get; set; }
        public bool isProzor { get; set; }
    }
}
