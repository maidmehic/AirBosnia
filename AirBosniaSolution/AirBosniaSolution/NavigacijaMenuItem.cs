﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirBosniaSolution
{

    public class NavigacijaMenuItem
    {
        public NavigacijaMenuItem()
        {
            TargetType = typeof(MainPage);
        }
        public string imageSource { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}