using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Responses.Equipment
{
    public sealed class AssignorListNotFoundResponse : ApiNotFoundResponse
    {
        public AssignorListNotFoundResponse() : base("No Assignors found in db.")
        {

        }
    }
    
}
