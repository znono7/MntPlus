using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public abstract class ApiErrorResponse : ApiBaseResponse
    {
        public string ErrorMessage { get; }

        protected ApiErrorResponse(string errorMessage) : base(false)
        {
            ErrorMessage = errorMessage;
        }
    }

}
