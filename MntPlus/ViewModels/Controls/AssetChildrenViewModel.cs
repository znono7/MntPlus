using Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MntPlus.WPF
{
    public class AssetChildrenViewModel : BaseViewModel
    {
       


        private ObservableCollection<AssetItemViewModel>? _children;
        public AssetChildrenViewModel()
        {
                     
           
                
            Children = new ObservableCollection<AssetItemViewModel>();
            
           
            
        }

       

      


        public ObservableCollection<AssetItemViewModel>? Children 
        {
            get { return _children; }
            set
            {
                _children = value;
                
                OnPropertyChanged(nameof(Children));
            }
        }

    }

   
}
