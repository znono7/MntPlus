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

            CreateMap<WorkOrderHistoryCreateDto, WorkOrderHistory>().ReverseMap();

            CreateMap<WorkOrderHistory, WorkOrderHistoryDto>().ReverseMap();

            CreateMap<Asset, AssetDto>().ReverseMap();
            CreateMap<AssetForCreationDto, Asset>().ReverseMap();

            CreateMap<LocationForCreationDto, Location>().ReverseMap();
            CreateMap<Location, LocationDto>().ReverseMap();

            //CreateMap<AssetForCreationDto, Asset>().ReverseMap();
            //CreateMap<Asset, AssetDto>()
            //    .ForMember(dest => dest.Location , opt => opt.MapFrom(src => src.Location)).ReverseMap();


            //CreateMap<UserCreateDto, User>().ReverseMap();
            //CreateMap<User, UserDto>().ReverseMap();

            //CreateMap<RoleDtoForCreation, Role>().ReverseMap();
            //CreateMap<Role, RoleDto>().ReverseMap();
            //CreateMap<UserRole, RoleDto>()
            //    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Role.Name))
            //    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Role.Id));
            //CreateMap<UserRole, UserRoleDto>().ReverseMap();

            ////CreateMap<User, UserWithRolesDto>()
            ////.ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));






            //CreateMap<TeamCreatedDto, Team>().ReverseMap();
            //CreateMap<Team, TeamDto>().ReverseMap();

            //CreateMap<WorkOrder, WorkOrderDto>()
            //.ForMember(dest => dest.UserAssignedTo, opt => opt.MapFrom(src => src.UserAssignedTo))
            //.ForMember(dest => dest.TeamAssignedTo, opt => opt.MapFrom(src => src.TeamAssignedTo))
            //.ForMember(dest => dest.Asset, opt => opt.MapFrom(src => src.Asset)).ReverseMap();
            //CreateMap<WorkOrderForCreationDto, WorkOrder>().ReverseMap();

            //CreateMap<WorkOrderHistory, WorkOrderHistoryDto>().ReverseMap();
            //CreateMap<WorkOrderHistoryCreateDto, WorkOrderHistory>().ReverseMap();

            //CreateMap<InstructionDtoForCreation, WorkTask>().ReverseMap();
            //CreateMap<WorkTask, InstructionDto>().ReverseMap();








        }
    }
}
