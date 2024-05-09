using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MntPlus.WPF
{
    public class InitialAssetViewModel : PropertyValidation
    {
        [Required(ErrorMessage = $"\uf06a  Nom de l'actif Doit être Précisé")]
        public string AssetName { get => GetValue(() => AssetName); set { SetValue(() => AssetName, value); } }

        [Required(ErrorMessage = $"\uf06a  Description de l'équipement Doit être Précisé")]
        public string Description { get => GetValue(() => Description); set { SetValue(() => Description, value); } }

        public ICommand AddCommand { get; set; }



       
        private bool _DimmableOverlayVisible { get; set;}

        public bool DimmableOverlayVisible { get => _DimmableOverlayVisible; set { _DimmableOverlayVisible = value; OnPropertyChanged(nameof(DimmableOverlayVisible)); } }

        public bool IsBtnEnabled => !DimmableOverlayVisible;

        public Guid? Parent { get; set; }
        public AssetStore? AssetStore2 { get; }
        private AssetStore _assetStore { get; set; }
        public InitialAssetViewModel(AssetStore assetStore, Guid? parent = null, AssetStore? assetStore2 = null)
        {
            AddCommand = new RelayParameterizedCommand( async (p) => await AddEquipment(p));
            _assetStore = assetStore;
            Parent = parent;
            AssetStore2 = assetStore2;
        }
       
        private async Task AddEquipment(object? p)
        {
           
            if (p is Window window)
            {
                if (IsValid)
                {
                    DimmableOverlayVisible = true;
                    var Result = await AppServices.ServiceManager.AssetService.CreateAsset(new AssetForCreationDto
                        (
                        AssetParent: Parent,
                            Name: AssetName,
                            Description: Description,
                            Status: null,
                            Category: null,
                            LocationId: null,
                            SerialNumber: null,
                            Model: null,
                            Make: null,
                            PurchaseCost: null,
                            ImagePath: null,
                            AssetImage: null,
                            AssetCommissionDate: null,null,null
                        ));
                          
                   
                   
                    if (Result is ApiOkResponse<AssetDto> okResponse)
                    {
                        if (Result.Success)
                        {
                            await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Success, "Actif Ajouté avec Succès"));
                        }
                        var AssetResult = okResponse.Result;
                        
                        if (AssetStore2 is not null)
                        {
                            AssetStore2.CreateAsset(AssetResult);
                        }
                        else
                        {
                            _assetStore.CreateAsset(AssetResult);
                        } 
                        await Task.Delay(200);
                        ClearValidation();
                        window.Close();
                    }
                    else if (Result is ApiBadRequestResponse errorResponse)
                    {
                        await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, errorResponse.Message)); //"Erreur lors de l'ajout de l'actif"));

                    }
                   
                    
                }else
                {
                   await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Veuillez Remplir les champs obligatoires"));
                }
                DimmableOverlayVisible = false;

            }

        }
       
    }
}
