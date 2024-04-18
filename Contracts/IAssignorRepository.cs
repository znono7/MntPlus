using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IAssignorRepository
    {
        Task<IEnumerable<Assignee>?> GetAllAssignorsAsync(bool trackChanges);
        Task<Assignee?> GetAssignorAsync(Guid assignorId, bool trackChanges);
        void CreateAssignor(Assignee assignor);
        void DeleteAssignor(Assignee assignor);
    }
}
