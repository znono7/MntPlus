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
            CreateMap<LocationForCreationDto, Location>().ReverseMap();
            CreateMap<Location, LocationDto>().ReverseMap();

            CreateMap<AssetForCreationDto, Asset>().ReverseMap();
            CreateMap<Asset, AssetDto>()
                .ForMember(dest => dest.Location , opt => opt.MapFrom(src => src.Location)).ReverseMap();


            CreateMap<UserCreateDto, User>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, UserWithRolesDto>()
                .ForMember(x => x.FullName, opt => opt.MapFrom(x => string.Join(' ',x.FirstName,x.LastName)))
                .ForMember(x => x.RoleNames, opt => opt.MapFrom(
                    src => string.Join(' ',  src.UserRoles != null ? string.Join(", ", src.UserRoles.Select(ur => ur.Role.Name)) : "")))
                .ReverseMap();


            CreateMap<TeamCreatedDto, Team>().ReverseMap();
            CreateMap<Team, TeamDto>().ReverseMap();

            CreateMap<WorkOrder, WorkOrderDto>()
            .ForMember(dest => dest.UserAssignedTo, opt => opt.MapFrom(src => src.UserAssignedTo))
            .ForMember(dest => dest.TeamAssignedTo, opt => opt.MapFrom(src => src.TeamAssignedTo))
            .ForMember(dest => dest.Asset, opt => opt.MapFrom(src => src.Asset)).ReverseMap();
            CreateMap<WorkOrderForCreationDto, WorkOrder>().ReverseMap();

            CreateMap<WorkOrderHistory, WorkOrderHistoryDto>().ReverseMap();
            CreateMap<WorkOrderHistoryCreateDto, WorkOrderHistory>().ReverseMap();

            CreateMap<InstructionDtoForCreation, Instruction>().ReverseMap();
            CreateMap<Instruction, InstructionDto>().ReverseMap();


           

        }
    }
}
