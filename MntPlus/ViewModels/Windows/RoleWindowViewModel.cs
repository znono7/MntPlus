using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MntPlus.WPF
{
    public class RoleWindowViewModel : BaseViewModel
    {
        public string? Name { get; set; }
        private bool _DimmableOverlayVisible { get; set; }

        public bool DimmableOverlayVisible { get => _DimmableOverlayVisible; set { _DimmableOverlayVisible = value; OnPropertyChanged(nameof(DimmableOverlayVisible)); } }

        public bool IsWorking { get; set; }

        public ICommand AddCommand { get; set; }
        public RoleStore? RoleStore { get; }
        public RoleWindowViewModel(RoleStore? roleStore)
        {
            RoleStore = roleStore;
            AddCommand = new RelayParameterizedCommand(async (p) => await Add(p));
            
        }

        private async Task Add(object? p)
        {
            if(p is null) return;
            if (!CanAdd())
            {
                await IoContainer.NotificationsManager.ShowMessage(new NotificationControlViewModel(NotificationType.Error, "Le Nom est requis!"));
                return;
            }
            var window = p as RoleWindow;
            await RunCommandAsync(() => IsWorking, async () =>
            {
                RoleStore?.CreateRole(new Shared.RoleDto(Guid.Empty, Name, false)) ;
                await Task.Delay(5);
            });
            window?.Close();
        }

        private bool CanAdd()
        {
            return !string.IsNullOrEmpty(Name);
        }
    }
}
