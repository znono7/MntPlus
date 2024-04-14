using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Responses.Equipment
{
    public sealed class EquipmentListNotFoundResponse : ApiNotFoundResponse
    {
        public EquipmentListNotFoundResponse() : base("No equipment found in db.")
        {

        }
    }
    
}
