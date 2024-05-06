using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public record AssetDto
    (
        Guid Id,
        Guid? AssetParent, 
        string? Name,
        string? Description,
        string? Status,
        string? Type,
        LocationDto? Location,
        string? SerialNumber,
        string? Model,
        string? Make,
        double? PurchaseCost, 
        string? ImagePath,
        byte[]? AssetImage,
        DateTime? AssetCommissionDate
    );

    public record AssetForCreationDto(
        Guid? AssetParent,
               string? Name, 
                      string? Description,
                             string? Status,
                                    string? Type,
                                           Guid? LocationId,
                                                  string? SerialNumber,
                                                         string? Model,
                                                                string? Make,
                                                                       double? PurchaseCost,
                                                                              string? ImagePath,
                                                                                     byte[]? AssetImage,
                                                                                            DateTime? AssetCommissionDate
           );

    public record AssetForUpdateImage(
               string? ImagePath,
                      byte[]? AssetImage
           );

}
