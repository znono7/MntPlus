

using System.Data;

namespace Shared
{
    public record UserDto
        (Guid Id, 
        string? FirstName, 
        string? LastName, 
        string? Email, 
        string? PhoneNumber,
        string? UserName, 
        
        string? Status,
        DateTime? CreatedAt,
        bool IsChecked);
    
    public record UserCreateDto(
         string? FirstName,
        string? LastName,
        string? Email,
        string? PhoneNumber,
        string? UserName,
        
        string? Status,
        DateTime? CreatedAt);

    // Create a record for User with all roles
    
    public record UserWithRolesDto(
               Guid Id,
               string? FullName,
               //string? Email,
               //string? PhoneNumber,
               //string? UserName,
               //string? Status,
               //DateTime? CreatedAt,
               //bool IsChecked,
               List<RoleDto> Roles,
               string RoleNames );
   
    


    


}
