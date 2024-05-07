using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MntPlus.WPF
{
    public class RoleStore
    {
        public event Action<RoleDto?>? RolrCreated;

        public void CreateRole(RoleDto? role)
        {
            RolrCreated?.Invoke(role);
        }

    }
}
