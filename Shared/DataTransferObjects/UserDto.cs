using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public record UserDto
        (Guid Id, 
        string FirstName, 
        string LastName, 
        string Email, 
        string UserName, 
        string Role);
    
    public record UserCreateDto(
        string FirstName, 
        string LastName, 
        string Email, 
        string UserName, 
        string Password, 
        string Role);



}
