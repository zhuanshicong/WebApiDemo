using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using IdentityMiddleware.IdentityProvider.Model;
using Microsoft.AspNetCore.Identity;
using MySql.Data.MySqlClient;

namespace IdentityMiddleware.IdentityProvider
{
    public class UserTable
    {
        private readonly MySqlConnection _connection;
        public UserTable(MySqlConnection connection)
        {
            _connection = connection;
            ConnectionOpen();
        }

        private void ConnectionOpen()
        {
            var retries = 3;
            try
            {
                if (_connection.State == ConnectionState.Open)
                {
                    return;
                }
                else
                {
                    while (retries >= 0 && _connection.State != ConnectionState.Open)
                    {
                        _connection.Open();
                        retries--;
                        Thread.Sleep(30);
                    }
                }
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e);
                throw;
            }

        }

        public void ConnectionClose()
        {
            try
            {
                if (_connection.State == ConnectionState.Open)
                {
                    _connection.Close();
                }
                _connection.Dispose();
                GC.Collect();
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e);
                throw;
            }

        }
        public async Task<IdentityResult> InsertAsync(UserModel user)
        {
            string sql = @"insert into useridentity.user values(@Id,@UserName,@NickName,@PasswordHash,@JoinDate,@Comments)";
            try
            {
                int rows = await _connection.ExecuteAsync(sql, new { Id = user.Id, UserName = user.UserName, NickName = user.NickName, PasswordHash = user.PasswordHash, JoinDate = DateTime.Now, Comments = user.Comments });
                if (rows > 0)
                {
                    return IdentityResult.Success;
                }
                return IdentityResult.Failed(new IdentityError { Description = $"Could not create user {user.UserName}." });
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
        public async Task<IdentityResult> DeleteAsync(UserModel user)
        {
            try
            {
                string sql = @"delete from useridentity.user where Id=@Id";
                int rows = await _connection.ExecuteAsync(sql, new { Id = user.Id });
                if (rows > 0)
                {
                    return IdentityResult.Success;
                }
                return IdentityResult.Failed(new IdentityError { Description = $"Could not delete user {user.UserName}." });
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
        public async Task<IdentityResult> UpdateAsync(UserModel user)
        {
            try
            {
                string sql = @"update useridentity.user set NickName=@NickName Comments=@Comments  where Id=@Id";
                int rows = await _connection.ExecuteAsync(sql, new {NickName=user.NickName,Comments=user.Comments, Id = user.Id });
                if (rows > 0)
                {
                    return IdentityResult.Success;
                }
                return IdentityResult.Failed(new IdentityError { Description = $"Could not update user {user.UserName}." });
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
        public async Task<UserModel> FindById(Guid userId)
        {
            try
            {
                string sql = @"select * from useridentity.user where Id=@Id;";
                var userResult = await _connection.QuerySingleAsync<UserModel>(sql, new { Id =userId });
                return userResult;
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
        public async Task<UserModel> FindByUserName(string userName)
        {
            try
            {
                string sql = @"select * from useridentity.user where UserName=@UserName;";
                var userResult = await _connection.QuerySingleOrDefaultAsync<UserModel>(sql, new { UserName = userName });
                return userResult;
                //return IdentityResult.Failed(new IdentityError { Description = $"Could not delete user {user.UserName}." });
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
        public async Task AddUserClaim(UserModel user, IEnumerable<Claim> claims)
        {
            //int result = 0;
            var enumerable = claims as Claim[] ?? claims.ToArray();
            var transaction = _connection.BeginTransaction();
            try
            {
                
                foreach (var item in enumerable)
                {
                    var sql = @"insert into useridentity.userclaim (UserId,UserName,ClaimType,ClaimValue) values(@i_UserId,@i_UserName,@i_ClaimType,@i_ClaimValue)";
                    var paras = new
                    {
                        i_UserId=user.Id,
                        i_UserName=user.UserName,
                        i_ClaimType=item.Type,
                        i_ClaimValue=item.Value
                    };
                    await _connection.ExecuteAsync(sql, paras, transaction);
                }
                transaction.Commit();
            }
            catch (MySqlException e)
            {
                transaction.Rollback();
                Console.WriteLine(e.Message);
                throw;
            }

            //return Task.FromResult<object>(null);
            //return result;
        }
        public async Task RemoveUserClaim(UserModel user, IEnumerable<Claim> claims)
        {
            //int result = 0;
            var enumerable = claims as Claim[] ?? claims.ToArray();
            var transaction = _connection.BeginTransaction();
            try
            {

                foreach (var item in enumerable)
                {
                    var sql = @"delete from useridentity.userclaim where UserId=@i_UserId and UserName=@i_UserName and ClaimType=@i_ClaimType and ClaimValue=@i_ClaimValue";
                    var paras = new
                    {
                        i_UserId = user.Id,
                        i_UserName = user.UserName,
                        i_ClaimType = item.Type,
                        i_ClaimValue = item.Value
                    };
                    await _connection.ExecuteAsync(sql, paras, transaction);
                }
                transaction.Commit();
            }
            catch (MySqlException e)
            {
                transaction.Rollback();
                Console.WriteLine(e.Message);
                throw;
            }

            //return Task.FromResult<object>(null);
            //return result;
        }
        public async Task UpdateUserClaim(UserModel user, Claim claim,Claim newClaim)
        {
            try
            {
                    var sql = @"update useridentity.userclaim set ClaimType=@i_NewClaimType,ClaimValue=@i_NewClaimValue where UserId=@i_UserId and UserName=@i_UserName and ClaimType =@i_ClaimType and ClaimValue=@i_ClaimValue;";
                    var paras = new
                    {
                        i_UserId = user.Id,
                        i_UserName = user.UserName,
                        i_ClaimType = claim.Type,
                        i_ClaimValue = claim.Value,
                        i_NewClaimType = newClaim.Type,
                        i_NewClaimValue = newClaim.Value
                    };
                    await _connection.ExecuteAsync(sql, paras);

            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
                throw;
            }

            //return Task.FromResult<object>(null);
            //return result;
        }
        public async Task<IList<Claim>> GetUserClaim(UserModel user)
        {
            var claimResultList=new List<Claim>();
            try
            {
                var sql = @"select UserId,UserName,ClaimType,ClaimValue from useridentity.userclaim where UserId=@i_UserId and UserName=@i_UserName";
                var paras = new
                {
                    i_UserId = user.Id,
                    i_UserName = user.UserName
                };
                var result = await _connection.QueryAsync(sql, paras);
                var enumerable = result as dynamic[] ?? result.ToArray();
                //if (!enumerable.Any())
                //{
                //    return ClaimResultList;
                //}
                claimResultList.AddRange(enumerable.Select(item => new Claim((string)item.ClaimType,(string)item.ClaimValue)));
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
                //return claimResultList;
                throw;
            }
            return claimResultList;
        }
        public async Task<IList<UserModel>> GetUserByClaim(Claim claim)
        {
            List<UserModel> result;
            try
            {
                const string sql = @"select user.Id,user.UserName,NickName,PasswordHash,JoinDate,Comments from useridentity.userclaim inner join useridentity.user on userclaim.UserId=user.Id and userclaim.UserName=user.UserName where ClaimType=@i_ClaimType and ClaimValue=@i_ClaimValue;";
                var paras = new
                {
                    i_ClaimType = claim.Type,
                    i_ClaimValue = claim.Value
                };
                var userResult = await _connection.QueryAsync<UserModel>(sql, paras);
                result=userResult.ToList();
                //return userResult.ToList();
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
                throw;
            }

            return result;
        }
        public async Task AddUserRole(UserModel user, string roleName)
        {
            try
            {
                var sql = @"insert into useridentity.userrole (UserId,UserName,RoleName)  values(@i_UserId,@i_UserName,@i_RoleName)";
                var paras = new
                {
                    i_UserId = user.Id,
                    i_UserName = user.UserName,
                    i_RoleName = roleName
                };
                await _connection.ExecuteAsync(sql, paras);
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public async Task RemoveUserRole(UserModel user, string roleName)
        {
            try
            {
                string sql = @"delete from useridentity.userrole where UserId=@i_UserId and UserName=@i_UserName and RoleName=@i_RoleName";
                var paras = new
                {
                    i_UserId = user.Id,
                    i_UserName = user.UserName,
                    i_RoleName = roleName
                };
                await _connection.ExecuteAsync(sql, paras);
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public async Task<IList<string>> GetUserRoles(UserModel user) 
        {
            var rolesResultList=new List<string>();
            try
            {
                string sql=@"select RoleName from useridentity.userrole where UserId=@i_UserId and UserName=@i_UserName";
                var  paras = new
                {
                    i_UserId=user.Id,
                    i_UserName=user.UserName
                };
                var resultTemp = await  _connection.QueryAsync<string>(sql, paras);
                rolesResultList.AddRange(resultTemp);
                //rolesResultList=resultTemp.ToList();
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
                throw;
            }

            return rolesResultList;
        }

        public async Task<bool> IsUserInRole(UserModel user, string roleName)
        {
            try
            {
                string sql = @"select count(*) as Count from useridentity.userrole roleTable inner join useridentity.user userTable on roleTable.UserId=userTable.Id and  roleTable.UserName=userTable.UserName where userTable.Id=@i_UserId and userTable.UserName=@i_UserName and RoleName=@i_RoleName";
                var paras = new
                {
                    i_UserId=user.Id,
                    i_UserName=user.UserName,
                    i_RoleName=roleName
                };
                var result = await _connection.QuerySingleAsync<int>(sql, paras);
                return result>0;
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }

        public async Task<IList<UserModel>> GetUsersInRole(string roleName)
        {
            var result=new List<UserModel>();
            try
            {
                string sql = @"select userTable.* from useridentity.userrole roleTable inner join useridentity.user userTable on roleTable.UserId=userTable.Id and  roleTable.UserName=userTable.UserName where RoleName=@i_RoleName";
                var paras = new
                {
                    i_RoleName = roleName
                };
                var queryTemp = await _connection.QueryAsync<UserModel>(sql, paras);
                result.AddRange(queryTemp);
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
                throw;
            }

            return result;
        }
    }
}
