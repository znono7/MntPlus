using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public sealed class EquipmentCreateErrorResponse : ApiErrorResponse
    {
        public EquipmentCreateErrorResponse(string errorMessage) : base($"An error occurred while creating equipment. Please try again later.{Environment.NewLine}{errorMessage}")
        {

        }
    }

    public sealed class EquipmentGetListErrorResponse : ApiErrorResponse
    {
        public EquipmentGetListErrorResponse(string errorMessage) : base($"An error occurred while Get equipments. Please try again later.{Environment.NewLine}{errorMessage}")
        {

        }
    }

    public sealed class EquipmentDeleteErrorResponse : ApiErrorResponse
    {
        public EquipmentDeleteErrorResponse(string errorMessage) : base($"An error occurred while Remove equipments. Please try again later.{errorMessage}")
        {

        }
    }
}
