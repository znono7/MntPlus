using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public sealed class EquipmentNotFoundException : NotFoundException
    {
        public EquipmentNotFoundException(Guid equipmentId) : 
            base($"The company with id: {equipmentId} doesn't exist in the database.")
        {
        }
    }

}
