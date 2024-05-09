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
        AssetDto? Parent,
        string? Name,
        string? Description,
        string? Status,
        string? Category,
        Guid? LocationId,
        LocationDto? Location,
        string? SerialNumber,
        string? Model,
        string? Make,
        double? PurchaseCost, 
        string? ImagePath,
        byte[]? AssetImage,
        DateTime? AssetCommissionDate,
        DateTime? CreatedDate,
        DateTime? PurchaseDate
    );

    public record AssetForCreationDto(
        Guid? AssetParent,
               string? Name, 
                      string? Description,
                             string? Status,
                                    string? Category,
                                           Guid? LocationId,
                                                  string? SerialNumber,
                                                         string? Model,
                                                                string? Make,
                                                                       double? PurchaseCost,
                                                                              string? ImagePath,
                                                                                     byte[]? AssetImage,
                                                                                            DateTime? AssetCommissionDate,
                                                                                            DateTime? CreatedDate,
                                                                                                             DateTime? PurchaseDate
           );

    public record AssetForUpdateImage(
               string? ImagePath,
                      byte[]? AssetImage
           );

}
