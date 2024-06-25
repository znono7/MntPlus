using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.IO;
using Shared;
using Entities;
using System.Drawing;

namespace MntPlus.WPF
{
    public class AssetItemViewModel : BaseViewModel
    { 
        #region Public Properties   
         
        /// <summary>  
        /// The display Description of Asset
        /// </summary>
        public string? AssetName { get; set; }
      


        /// <summary>
        /// True if this item is currently selected
        /// </summary>
        public bool IsSelected { get; set; }

       
        #endregion



        #region Constructor

        public AssetItemViewModel()
        {
        }

       

       

        #endregion

    }

    
}
