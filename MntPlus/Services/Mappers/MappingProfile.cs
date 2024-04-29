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
            CreateMap<LocationForCreationDto, Location>();
            CreateMap<Location, LocationDto>();

            CreateMap<AssetForCreationDto, Asset>();
            CreateMap<Asset, AssetDto>();
        }
    }
}
