using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMiddleware.IdentityProvider.Model
{
    public class RoleModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string RoleName { get; set; }
    }
}
