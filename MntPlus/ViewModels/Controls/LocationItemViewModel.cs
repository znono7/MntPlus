using Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MntPlus.WPF
{
    public class LocationItemViewModel : BaseViewModel
    {
       
        public Guid Id { get; set; }

        public string Name { get; set; }
               
        public string? Address { get; set; }
        public bool IsPrimaryLocation { get; set; }
               
        public Guid? IdParent { get; set; }
               
        public LocationDto? Parent {  get; set; }
               
        public DateTime CreatedAt { get; set; }

        public LocationDto? LocationDto { get; set; }

        private ObservableCollection<LocationItemViewModel>? _children;

        public ObservableCollection<LocationItemViewModel>? Children
        {
            get { return _children; }
            set
            {
                _children = value;
                OnPropertyChanged(nameof(Children));
            }
        }
        public LocationItemViewModel(LocationDto? locationDto)
        {
            LocationDto = locationDto;
            Children = new ObservableCollection<LocationItemViewModel>();
            Id = locationDto.Id;  
            IdParent = locationDto.IdParent;
            Parent = locationDto.Parent ;
            Name = locationDto.Name;
            Address = locationDto.Address;
            CreatedAt = locationDto.CreatedAt;
            IsPrimaryLocation = locationDto.IsPrimaryLocation;


        }

    }
}
