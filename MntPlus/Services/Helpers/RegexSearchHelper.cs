using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MntPlus.WPF
{
    public class RegexSearchHelper
    {
        public IEnumerable<string> SearchItems(List<string> items, string searchPattern)
        {
            string regexPattern = "^" + Regex.Escape(searchPattern).Replace("\\*", ".*") + "$";
            Regex regex = new Regex(regexPattern);

            return items.FindAll(item => regex.IsMatch(item));
        }
    }
}
