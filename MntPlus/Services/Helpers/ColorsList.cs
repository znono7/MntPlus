using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MntPlus.WPF
{
    public class ColorsList
    {
        public List<string> Colors { get; set; }
        public ColorsList()
        {
            Colors = new List<string> 
            {
                "D4F1F4","75E6DA","189AB4","05445E","FFCDB2","FFB4A2","E5989B","B5838D","6D6875",
                "94C973","4F9D69","2B580C","FFA900",
                "B1D8B7","7EBDC2","3A95A8","2A6E78","F2D7D5","CD6155",
            };
            
        }
    }
}
