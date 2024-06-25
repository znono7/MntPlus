using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MntPlus.WPF
{
    public class RequestStore
    {
        public event Action<RequestDto?>? RequestCreated;
        public event Action<RequestDto?>? RequestUpdated;
        public event Action<RequestDto?>? RequestRemoved;

        public void CreateRequest(RequestDto? request)
        {
            RequestCreated?.Invoke(request);
        }

        public void UpdateRequest(RequestDto? request)
        {
            RequestUpdated?.Invoke(request);
        }

        public void RemoveRequest(RequestDto? request)
        {
            RequestRemoved?.Invoke(request);
        }

    }
}
