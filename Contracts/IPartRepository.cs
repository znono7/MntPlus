using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IPartRepository
    {
        Task<IEnumerable<Part>?> GetAllPartsAsync(bool trackChanges);
        Task<Part?> GetPartAsync(Guid partId, bool trackChanges);
        void CreatePart(Part part);
        void DeletePart(Part part); 
    }
}
