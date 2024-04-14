using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public sealed class EquipmentNotFoundResponse : ApiNotFoundResponse
    {
        public EquipmentNotFoundResponse(Guid id) : base($"Equipment with id: {id} is not found in db.")
        {
        }
    }
   
}
