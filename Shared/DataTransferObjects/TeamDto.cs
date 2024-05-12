using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public record TeamDto (Guid Id,string Name,bool IsChecked);

    public record TeamCreatedDto(string Name);
    
}
