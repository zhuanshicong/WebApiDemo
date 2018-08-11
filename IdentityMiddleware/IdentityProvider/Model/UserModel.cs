using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace IdentityMiddleware.IdentityProvider.Model
{
    public class UserModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string UserName { get; set; }

        public string NickName { get; set; }

        public  string PasswordHash { get; set; }

        public DateTime JoinDate { get; set; }

        public string Comments { get; set; }
    }

    public class RegisterModel
    {
        [Required]
        public string UserName { get; set; }
        public string NickName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "两次输入的密码不一致!")]
        public string ConfirmPassword { get; set; }


    }

    public class AddUserRoleModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string[] RoleNames { get; set; }
    }
}
