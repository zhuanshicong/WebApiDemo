using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace IdentityMiddleware.IdentityProvider.Model
{
    public class UserModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string UserName { get; set; }

        public string NickName { get; set; }

        public  string PasswordHash { get; set; }

        public DateTime JoinDate { get; set; }

        public string Comments { get; set; }
    }
}
