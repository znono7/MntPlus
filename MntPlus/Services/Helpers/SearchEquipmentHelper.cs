﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MntPlus.WPF
{
    public class SearchEquipmentHelper
    {
        public IEnumerable<AssetItemViewModel> SearchItems(ObservableCollection<AssetItemViewModel> items, string searchPattern)
        {
            // Case 1: Search for items that end with the word
            if (searchPattern.StartsWith("*") && !searchPattern.EndsWith("*"))
            {
                string searchTerm = searchPattern.TrimStart('*');
                return items.Where(item => item.AssetName!.EndsWith(searchTerm, StringComparison.OrdinalIgnoreCase));
            }
            // Case 2: Search for items that start with the word
            else if (!searchPattern.StartsWith("*") && searchPattern.EndsWith("*"))
            {
                string searchTerm = searchPattern.TrimEnd('*');
                return items.Where(item => item.AssetName!.StartsWith(searchTerm, StringComparison.OrdinalIgnoreCase));
            }
            // Case 3: Search for items that contain the word
            else if (searchPattern.StartsWith("*") && searchPattern.EndsWith("*"))
            {
                string searchTerm = searchPattern.Trim('*');
                return items.Where(item => item.AssetName!.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
            }
            else
            {
                // Invalid search pattern
                return Enumerable.Empty<AssetItemViewModel>();
            }
        }
    }
}
