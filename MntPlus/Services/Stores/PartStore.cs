using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MntPlus.WPF
{
    public class PartStore
    {
        public event Action<PartDto?>? PartCreated;

        public void CreatePart(PartDto? part)
        {
            PartCreated?.Invoke(part);
        }
    }
}
