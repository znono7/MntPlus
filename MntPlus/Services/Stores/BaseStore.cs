using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MntPlus.WPF
{
    public class BaseStore<TDto>
    {
        public event Action<TDto?>? ItemCreated;

        public void CreateItem(TDto? item)
        {
            ItemCreated?.Invoke(item);
        }
    }
}
