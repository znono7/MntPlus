using Entities;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public interface IAssignorService
    {
        Task<ApiBaseResponse> GetAllAssignorsAsync(bool trackChanges);
        Task<ApiBaseResponse> GetAssignorAsync(Guid assignorId, bool trackChanges);
        Task<ApiBaseResponse> CreateAssignor(AssignedToForCreationDto assignor); 
        Task<ApiBaseResponse> DeleteAssignor(Guid assignorId, bool trackChanges);
    }
}
