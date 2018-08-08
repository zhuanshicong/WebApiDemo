using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using IdentityMiddleware.IdentityProvider.Model;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Identity;

namespace IdentityMiddleware.IdentityProvider
{
    public class UserStore: IUserStore<UserModel>,
                            IUserClaimStore<UserModel>,
                            IUserRoleStore<UserModel>,
                            IUserPasswordStore<UserModel>
    {
        private readonly UserTable _userTable;
        public UserStore(UserTable userTable)
        {
            _userTable = userTable;
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public Task<string> GetUserIdAsync(UserModel user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.Id.ToString());
        }

        public Task<string> GetUserNameAsync(UserModel user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.UserName);
        }

        public Task SetUserNameAsync(UserModel user, string userName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (userName == null) throw new ArgumentNullException(nameof(userName));
            user.UserName = userName;
            return Task.FromResult<object>(null);
        }

        public Task<string> GetNormalizedUserNameAsync(UserModel user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(UserModel user, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            //if (user == null) throw new ArgumentNullException(nameof(user));
            //if (normalizedName == null) throw new ArgumentNullException(nameof(normalizedName));
            //user.UserName = normalizedName;
            return Task.FromResult<object>(null);
        }

        public Task<IdentityResult> CreateAsync(UserModel user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            return _userTable.InsertAsync(user);
            //throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(UserModel user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            return _userTable.UpdateAsync(user);
            //throw new NotImplementedException();
        }

        public Task<IdentityResult> DeleteAsync(UserModel user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            return _userTable.DeleteAsync(user);
            //throw new NotImplementedException();
        }

        public Task<UserModel> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (userId == null) throw new ArgumentNullException(nameof(userId));
            if (!Guid.TryParse(userId, out var idGuid))
            {
                throw new ArgumentException("Not a valid Guid id", nameof(userId));
            }
            return _userTable.FindById(idGuid);
            //throw new NotImplementedException();
        }

        public Task<UserModel> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (normalizedUserName == null) throw new ArgumentNullException(nameof(normalizedUserName));
            return _userTable.FindByUserName(normalizedUserName);
            //throw new NotImplementedException();
        }

        public Task<IList<Claim>> GetClaimsAsync(UserModel user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            return _userTable.GetUserClaim(user);
            //throw new NotImplementedException();
        }

        public Task AddClaimsAsync(UserModel user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (claims == null) throw new ArgumentNullException(nameof(claims));
            return _userTable.AddUserClaim(user,claims);
            //throw new NotImplementedException();
        }

        public Task ReplaceClaimAsync(UserModel user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (claim == null) throw new ArgumentNullException(nameof(claim));
            if (newClaim == null) throw new ArgumentNullException(nameof(newClaim));
            return _userTable.UpdateUserClaim(user, claim, newClaim);
        }

        public Task RemoveClaimsAsync(UserModel user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (claims == null) throw new ArgumentNullException(nameof(claims));
            return _userTable.RemoveUserClaim(user, claims);
           
        }

        public Task<IList<UserModel>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (claim == null) throw new ArgumentNullException(nameof(claim));
            return _userTable.GetUserByClaim(claim);
            //throw new NotImplementedException();
        }

        public Task AddToRoleAsync(UserModel user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (roleName == null) throw new ArgumentNullException(nameof(roleName));
            if (user == null) throw new ArgumentNullException(nameof(user));
            return _userTable.AddUserRole(user, roleName);
        }

        public Task RemoveFromRoleAsync(UserModel user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (roleName == null) throw new ArgumentNullException(nameof(roleName));
            if (user == null) throw new ArgumentNullException(nameof(user));
            return _userTable.RemoveUserRole(user, roleName);
        }

        public Task<IList<string>> GetRolesAsync(UserModel user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            return _userTable.GetUserRoles(user);
            //throw new NotImplementedException();
        }

        public Task<bool> IsInRoleAsync(UserModel user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (roleName == null) throw new ArgumentNullException(nameof(roleName));
            return _userTable.IsUserInRole(user, roleName);
            //throw new NotImplementedException();
        }

        public Task<IList<UserModel>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (roleName == null) throw new ArgumentNullException(nameof(roleName));
            return _userTable.GetUsersInRole(roleName);
        }

        public Task SetPasswordHashAsync(UserModel user, string passwordHash, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (passwordHash == null) throw new ArgumentNullException(nameof(passwordHash));
            user.PasswordHash = passwordHash;
            return Task.FromResult<object>(null);
        }

        public Task<string> GetPasswordHashAsync(UserModel user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(UserModel user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (user == null) throw new ArgumentNullException(nameof(user));
            return Task.FromResult(!user.PasswordHash.IsNullOrEmpty());
            //throw new NotImplementedException();
        }
    }
}
