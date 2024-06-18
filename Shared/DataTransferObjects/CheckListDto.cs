using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public record CheckListDto
    (
        Guid Id,
        string? Description,
        string? Name
        );
    public record CreateCheckListDto 
   (
       string? Description,
       string? Name
       );

    public record IndividualTaskDto
    (
               Guid Id
                     
               );
    public record CreateIndividualTaskDto
        (
               );


    public record CheckListItemDto
        (
        Guid Id,
        int Order,
            string? Description 
        );
    public record CreateCheckListItemDto 
        (
                   int Order, string? Description , Guid? CheckListId , Guid? IndividualTaskId
               );
    public record TaskItem(
        string? Description,
        string? Number
        );
        
}
