using Entities;
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
        public ObservableCollection<AssetItemViewModel>? Children
        {
            get { return _children; }
            set
            {
                _children = value;

                OnPropertyChanged(nameof(Children));
            }
        }


        public EquipmentItemViewModel EquipmentItem { get; set; }
       
        public AssetChildrenViewModel(EquipmentItemViewModel equipmentItem)
        {
            Children = new ObservableCollection<AssetItemViewModel>();
            EquipmentItem = equipmentItem;
            OrganizeData();
        }
        private void OrganizeData()
        {
            EquipmentHierarchyManager manager = new EquipmentHierarchyManager();
            manager.GetFullHierarchy(EquipmentItem);
            if (manager.EquipmentHierarchy != null)
            {
                Children = new ObservableCollection<AssetItemViewModel>(FromEquipToAsset(manager.EquipmentHierarchy));
            }
        }
        public List<AssetItemViewModel> FromEquipToAsset(List<EquipmentItemViewModel> assetItem)
        {
            List<AssetItemViewModel> assetItems = new();
            foreach (var item in assetItem)
            {
                if (item.Asset?.Id == EquipmentItem.Asset?.Id)
                {
                    AssetItemViewModel asset = new()
                    {
                        AssetName = $"Cette équipement : {item.Name}"
                    };
                    assetItems.Add(asset);

                }
                else
                {
                    AssetItemViewModel asset = new()
                    {
                        AssetName = item.Name
                    };
                    assetItems.Add(asset);

                }

            }
            return assetItems;

        }







    }


}
