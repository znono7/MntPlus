﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public record LocationDto(
               Guid Id,
               string Name,
               string Address,
               string City,
               string State,
               string Country,
               string ImagePath,
               byte[] LocationImage
               );

    public record LocationForCreationDto
        (
        string? Name,
            string? Address,
            string? City,
            string? State,
            string? Country,
            string? ImagePath,
            byte[]? LocationImage
        );

   
    
}
