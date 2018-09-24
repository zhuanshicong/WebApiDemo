using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using IdentityMiddleware.Helper;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Type = Google.Protobuf.WellKnownTypes.Type;

namespace IdentityMiddleware.IdentityProvider.Model
{
    public class ClaimModel
    {
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
    public class EditUserClaimModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public IEnumerable<ClaimModel> ClaimModel { get; set; }
        [Required]
        [EnumValueValidation(typeof(EditUserClaimType))]
        public EditUserClaimType EditUserClaimType { get; set; }

    }

    public class ChangePasswordModel
    {
        [Required]
        public string UserName { get;set; }
        [Required]
        public string OldPassword { get; set; }
       [Required]
        public string NewPassword { get; set; }
    }
    public class ResetPasswordModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string NewPassword { get; set; }
    }
    public enum EditUserClaimType
    {
        Add,Delete,
    }
}
