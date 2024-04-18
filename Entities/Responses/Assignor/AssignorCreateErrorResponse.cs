using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public sealed class AssignorCreateErrorResponse : ApiErrorResponse
    {
        public AssignorCreateErrorResponse(string errorMessage) : base($"An error occurred while creating Assignor. Please try again later.")
        {

        }
    }

    public sealed class AssignorGetListErrorResponse : ApiErrorResponse
    {
        public AssignorGetListErrorResponse(string errorMessage) : base($"An error occurred while Get Assignors. Please try again later.")
        {

        }
    }

    public sealed class AssignorDeleteErrorResponse : ApiErrorResponse
    {
        public AssignorDeleteErrorResponse(string errorMessage) : base($"An error occurred while Remove Assignor. Please try again later.")
        {

        }
    }
}
