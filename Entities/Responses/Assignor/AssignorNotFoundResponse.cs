using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public sealed class AssignorNotFoundResponse : ApiNotFoundResponse
    {
        public AssignorNotFoundResponse(string id) : base($"Assignor with nqme: {id} is not found in db.")
        {
        }
    }
   
}
