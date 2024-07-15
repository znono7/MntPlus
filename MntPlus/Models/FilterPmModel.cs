using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MntPlus.WPF
{
    public class FilterPmModel
    {
        public string? Field { get; set; }
        public string? Operator { get; set; }
        public string? ValueType { get; set; }
        public object? Value { get; set; }
    }
}
