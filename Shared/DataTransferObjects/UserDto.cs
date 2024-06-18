

using System.Data;

namespace Shared
{
    public record UserDto
        (Guid Id, 
        string? FullName, 
        string? Email, 
        string? PhoneNumber,
        string? UserName, 
        string? Status,
        DateTime? CreatedAt,
        bool IsChecked);
    public record UserByDto
       (Guid Id,
       string? FullName
      );

    public record UserCreateDto(
         string? FirstName,
        string? LastName,
        string? Email,
        string? PhoneNumber,
        string? UserName,
        string? Password,
        string? Status,
        DateTime? CreatedAt);

      
    public record UserWithRolesDto(
               UserDto? UserDto,
               List<RoleDto?>? UserRoles
                );

   




}
