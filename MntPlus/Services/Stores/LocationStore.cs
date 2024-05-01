using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MntPlus.WPF
{
    public class LocationStore
    {
        public event Action<LocationDto?>? LocationCreated;

        public void CreateLocation(LocationDto? location)
        {
            LocationCreated?.Invoke(location);
        }

    }
}
