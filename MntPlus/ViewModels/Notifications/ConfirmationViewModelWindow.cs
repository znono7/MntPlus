using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MntPlus.WPF
{
    public class ConfirmationViewModelWindow : BaseViewModel
    {
        public  string? AssetName { get;  set; }
        public string Title { get; set; }
        public Window Window { get; }
        public string? Message { get; set; }
        

        //delete command
        public ICommand DeleteCommand { get; set; }
        public ICommand CloseCommand { get; set; }

        public ConfirmationViewModelWindow(Window window, string? asset, string? message)
        {
            AssetName = asset;
            Window = window;
            Message = message;
            Title = $"Supprimer l'équipement - {AssetName}";
            DeleteCommand = new RelayCommand(async () => await Delete());
            CloseCommand = new RelayCommand(() => Window.Close());
        }

        private async Task Delete()
        {
            //close the window
            Window.Close();
            //delete the equipment
            //await IoC.Equipment.DeleteAsync(EquipmentName);
            await Task.Delay(1);
        }
    }
}
