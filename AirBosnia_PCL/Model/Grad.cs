using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirBosnia_PCL.Model
{
   public class Grad
    {
        public int GradID { get; set; }
        public string Naziv { get; set; }
        public string Oznaka { get; set; }

        public string Prikazi
        {
            get { return Naziv +" ("+Oznaka+")"; }
        }
    }
}
