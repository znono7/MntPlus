using AutoMapper;
using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MntPlus.WPF
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.IsChecked, opt => opt.Ignore());

            CreateMap<Role, RoleDto>().ReverseMap();

            CreateMap<UserRole, UserRoleDto>().ReverseMap();

            CreateMap<RoleForUserCreationDto , UserRole>().ReverseMap();
            
            CreateMap<WorkOrderForCreationDto, WorkOrder>().ReverseMap();
            CreateMap<WorkOrder, WorkOrderDto>().ReverseMap();

            CreateMap<WorkOrderHistory, WorkOrderHistoryDto>().ReverseMap();
            CreateMap<WorkOrderHistoryCreateDto, WorkOrderHistory>().ReverseMap();


            CreateMap<Asset, AssetDto>().ReverseMap();
            CreateMap<AssetForCreationDto, Asset>().ReverseMap();

            CreateMap<LocationForCreationDto, Location>().ReverseMap();
            CreateMap<Location, LocationDto>().ReverseMap();

            CreateMap<Part, PartDto>().ReverseMap();
            CreateMap<PartForCreationDto, Part>().ReverseMap();

            CreateMap<Inventory, InventoryDto>().ReverseMap();
            CreateMap<InventoryForCreationDto, Inventory>().ReverseMap();

            CreateMap<Meter,MeterDto>()
                .ForMember(dest => dest.AssetName, opt => opt.MapFrom(src => src.Asset!.Name))
                .ReverseMap();
            CreateMap<MeterDtoForCreation, Meter>().ReverseMap();

            CreateMap<MeterReading, MeterReadingDto>()
                .ForMember(dest => dest.UserFullName, opt => opt.MapFrom(src => src.User != null ? $"{src.User!.FirstName} {src.User.LastName}" : string.Empty))
                .ReverseMap();
            CreateMap<MeterReadingDtoForCreation, MeterReading>().ReverseMap();




            CreateMap<RequestForCreationDto, Request>().ReverseMap();
            CreateMap<Request, RequestDto>().ReverseMap();


        }
    }
}
