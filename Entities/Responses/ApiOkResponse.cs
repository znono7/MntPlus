using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public sealed class ApiOkResponse<TResult> : ApiBaseResponse
    {
        public TResult? Result { get; }

        public ApiOkResponse(TResult? result) : base(true) => Result = result;
    }
    
}
