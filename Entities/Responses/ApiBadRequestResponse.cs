using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public sealed class ApiBadRequestResponse : ApiBaseResponse
    {
        public string Message { get; set; }
        public ApiBadRequestResponse(string message) : base(false)
        {
            Message = message;
        } 

    }

    public sealed class ApiPropertyNullRequestResponse : ApiBaseResponse
    {
        public string Message { get; set; }
            
        public ApiPropertyNullRequestResponse(string message) : base(false)
        {
            Message = message;
        }

    }
}
