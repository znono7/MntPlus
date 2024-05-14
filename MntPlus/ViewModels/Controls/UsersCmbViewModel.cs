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
    public class UsersCmbViewModel : BaseViewModel
    {
        public ObservableCollection<UserWithRolesDto> FilterUsers { get; set; } 
        private ObservableCollection<UserWithRolesDto> users { get; set; } 
        public ObservableCollection<UserWithRolesDto> Users
        {
            get => users;
            set
            {
                users = value;
                if (users is not null)
                {
                    FilterUsers = new ObservableCollection<UserWithRolesDto>(users);
                }
            }
        }
        public bool AttachmentMenuVisible { get; set; }
        public ICommand AttachmentButtonCommand { get; set; }

        public UsersCmbViewModel()
        {
            AttachmentButtonCommand = new RelayCommand(async() => await AttachmentButton());
        }

        //generate Users Data 
        public void GenerateUsers()
        {
            Users = new ObservableCollection<UserWithRolesDto>
            {
                //new UserWithRolesDto(Guid.Empty , "User 1" , new List<RoleDto>
                //{
                //    new RoleDto(Guid.Empty,"Manager",true),
                //    new RoleDto(Guid.Empty,"Admin",true),
                //} , "Manager, Admin"),
                //new UserWithRolesDto(Guid.Empty , "User 2" , new List<RoleDto>
                //{
                //    new RoleDto(Guid.Empty,"Manager",true),
                //    new RoleDto(Guid.Empty,"Admin",true),
                //} , "Manager, Admin"),
                //new UserWithRolesDto(Guid.Empty , "User 3" , new List<RoleDto>
                //{
                //    new RoleDto(Guid.Empty,"Manager",true),
                //    new RoleDto(Guid.Empty,"Admin",true),
                //} , "Manager, Admin"),
                
            };
        }

        public async Task AttachmentButton()
        {

            // Toggle menu visibility
            AttachmentMenuVisible ^= true;
           await Task.Delay(1);

            //try
            //{
            //    await
            //    FetchClientsFromDatabase();


            //}
            //catch (Exception)
            //{

            //    throw;
            //}


        }

    }
}
