using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MntPlus.WPF
{
    public class SubLocationViewModel
    {
        public LocationDto? Location { get; set; }
        public string? Name => Location?.Name;
        public string? Adress => Location?.Address;
        public string? CreatedAt => Location?.CreatedAt.ToShortDateString();

        public SubLocationViewModel(LocationDto? location)
        {
            Location = location;
        }
    }
}
