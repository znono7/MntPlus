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
    public class SettingsViewModel : BaseViewModel
    {
        public List<RoleDto> SeedsRoles { get; set; }

        public ObservableCollection<RoleDto> Roles { get; set; }
        public ObservableCollection<LocationDto> LocationDtos { get; set; } 

        public bool DeleteRoleIsRunning { get; set; }
         
        public ICommand RemoveRoleCommand { get; set; }
        public ICommand AddRoleCommand { get; set; }
        public ICommand AddLocationCommand { get; set; }
        public RoleStore? RoleStore { get; }
        public LocationStore? LocationStore { get; set; }

        public SettingsViewModel()
        {
            SeedsRoles = new List<RoleDto>
            {
                new RoleDto(Guid.NewGuid(), "Ingénieur GMAO", true),
                new RoleDto(Guid.NewGuid(), "Responsable", true),
                new RoleDto(Guid.NewGuid(), "Demandeur", true),
            };

            Roles = new ObservableCollection<RoleDto>();
            RemoveRoleCommand = new RelayParameterizedCommand(async (p) => await RemoveRole(p));
            LocationDtos = new ObservableCollection<LocationDto>();
            AddRoleCommand = new RelayCommand(AddRole);
            AddLocationCommand = new RelayCommand(AddLocation);
            RoleStore = new RoleStore();
            RoleStore.RolrCreated += RoleStore_RolrCreated;
            LocationStore = new LocationStore();
            LocationStore.LocationCreated += LocationStore_LocationCreated;
        }

        private void AddLocation()
        {
            LocationWindow locationWindow = new() { DataContext = new LocationWindowViewModel(LocationStore) };
            locationWindow.ShowDialog();
            
        }

        private void LocationStore_LocationCreated(LocationDto? obj)
        {
            LocationDtos?.Add(obj!);
        }

        private void RoleStore_RolrCreated(RoleDto? dto)
        {
            Roles?.Add(dto!);
        }

        private void AddRole()
        {
            RoleWindow roleWindow = new() { DataContext = new RoleWindowViewModel(RoleStore) };
            roleWindow.ShowDialog();
        }

        private async Task RemoveRole(object? p)
        {
            if (p is null) return;
            if (p is not RoleDto) return;
            var role = p as RoleDto;
            await RunCommandAsync(() => DeleteRoleIsRunning, async () =>
            {
               
                Roles.Remove(role);
                await Task.Delay(5);
            });
        }
    }
}
