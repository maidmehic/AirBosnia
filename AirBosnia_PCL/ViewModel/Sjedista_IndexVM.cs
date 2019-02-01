using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirBosnia_PCL.ViewModel
{
   public class Sjedista_IndexVM
    {
        public class Rows
        {
            public int SjedisteID { get; set; }
            public string Oznaka { get; set; }
            public int LetID { get; set; }
            public bool isProzor { get; set; }

            public string OznakaSaProzorom { get {

                    if (isProzor == true)
                    {
                        return Oznaka + " *prozor*";
                    }
                    else
                    {
                        return Oznaka;
                    }

                } set { } }
        }

        public List<Rows> sjedista { get; set; }
    }
}
