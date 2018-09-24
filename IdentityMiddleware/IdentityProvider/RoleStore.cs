using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using IdentityMiddleware.IdentityProvider.Model;
using Microsoft.AspNetCore.Identity;

namespace IdentityMiddleware.IdentityProvider
{
    public class RoleStore:IRoleStore<RoleModel>/*,IRoleClaimStore<RoleModel>*/
    {
        private readonly RoleTable _roleTable;

        public RoleStore(RoleTable roleTable)
        {
            _roleTable = roleTable;
        }
        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        public Task<IdentityResult> CreateAsync(RoleModel role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null) throw new ArgumentNullException(nameof(role));
            return _roleTable.InsertAsync(role);
            //throw new NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(RoleModel role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null) throw new ArgumentNullException(nameof(role));
            return _roleTable.UpdateAsync(role);
            
            //throw new NotImplementedException();
        }

        public Task<IdentityResult> DeleteAsync(RoleModel role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null) throw new ArgumentNullException(nameof(role));
            return _roleTable.DeleteAsync(role);
            //throw new NotImplementedException();
        }

        public Task<string> GetRoleIdAsync(RoleModel role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null) throw new ArgumentNullException(nameof(role));
            return Task.FromResult(role.Id.ToString());
            //throw new NotImplementedException();
        }

        public Task<string> GetRoleNameAsync(RoleModel role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null) throw new ArgumentNullException(nameof(role));
            return Task.FromResult(role.RoleName);
            //throw new NotImplementedException();
        }

        public Task SetRoleNameAsync(RoleModel role, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null) throw new ArgumentNullException(nameof(role));
            if (roleName == null) throw new ArgumentNullException(nameof(roleName));
            role.RoleName = roleName;
            return Task.FromResult<object>(null);
            //throw new NotImplementedException();
        }

        public Task<string> GetNormalizedRoleNameAsync(RoleModel role, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null) throw new ArgumentNullException(nameof(role));
            return Task.FromResult(role.RoleName);
            //throw new NotImplementedException();
        }

        public Task SetNormalizedRoleNameAsync(RoleModel role, string normalizedName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (role == null) throw new ArgumentNullException(nameof(role));
            if (normalizedName == null) throw new ArgumentNullException(nameof(normalizedName));
            role.RoleName = normalizedName;
            return Task.FromResult<object>(null);
            //throw new NotImplementedException();
        }

        public Task<RoleModel> FindByIdAsync(string roleId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (roleId == null) throw new ArgumentNullException(nameof(roleId));
            return _roleTable.FindByIdAsync(roleId);
            //throw new NotImplementedException();
        }

        public Task<RoleModel> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (normalizedRoleName == null) throw new ArgumentNullException(nameof(normalizedRoleName));
            return _roleTable.FindByNameAsync(normalizedRoleName);
            //throw new NotImplementedException();
        }

        //public Task<IList<Claim>> GetClaimsAsync(RoleModel role, CancellationToken cancellationToken = new CancellationToken())
        //{

        //    throw new NotImplementedException();
        //}

        //public Task AddClaimAsync(RoleModel role, Claim claim, CancellationToken cancellationToken = new CancellationToken())
        //{
        //    throw new NotImplementedException();
        //}

        //public Task RemoveClaimAsync(RoleModel role, Claim claim, CancellationToken cancellationToken = new CancellationToken())
        //{
        //    throw new NotImplementedException();
        //}
    }
}
