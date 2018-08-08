using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using IdentityMiddleware.IdentityProvider.Model;
using Microsoft.AspNetCore.Identity;
using MySql.Data.MySqlClient;

namespace IdentityMiddleware.IdentityProvider
{
    public class RoleTable
    {
        private readonly MySqlConnection _connection;
        public RoleTable(MySqlConnection connection)
        {
            _connection = connection;
            //_connection.Open();
        }
        public async Task<IdentityResult> InsertAsync(RoleModel role)
        {
            string sql = @"insert into useridentity.role (Id,RoleName) values(@i_Id,@i_RoleName)";
            try
            {
                int rows = await _connection.ExecuteAsync(sql, new {i_Id=role.Id,i_RoleName=role.RoleName });
                if (rows > 0)
                {
                    return IdentityResult.Success;
                }
                return IdentityResult.Failed(new IdentityError { Description = $"Could not create role {role.RoleName}." });
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
                switch (e.Number)
                {
                    case 1062: return IdentityResult.Failed(new IdentityError { Description = $"Could not create a exist role {role.RoleName}." });
                    default:return IdentityResult.Failed(new IdentityError { Description = $"Could not create role {role.RoleName}." });
                }
            }
        }
        public async Task<IdentityResult> UpdateAsync(RoleModel role)
        {
            string sql = @"update useridentity.role set RoleName=@i_RoleName where Id=@i_Id;";
            try
            {
                int rows = await _connection.ExecuteAsync(sql, new { i_Id = role.Id, i_RoleName = role.RoleName });
                if (rows > 0)
                {
                    return IdentityResult.Success;
                }
                return IdentityResult.Failed(new IdentityError { Description = $"No RoleId => {role.Id} matched." });
            }
            catch (MySqlException e)
            {

                Console.WriteLine(e);
                switch (e.Number)
                {
                    case 1761: return IdentityResult.Failed(new IdentityError { Description = $"Could not update a  role name =>{role.RoleName} to a exist name" });
                    case 1062: return IdentityResult.Failed(new IdentityError { Description = $"Could not update a  role name =>{role.RoleName} to a exist name" });
                    default: return IdentityResult.Failed(new IdentityError { Description = $"Could not update role {role.Id}." });
                }
            }

        }
        public async Task<IdentityResult> DeleteAsync(RoleModel role)
        {
            try
            {
                string sql = @"delete from useridentity.role where Id=@i_Id;";
                int rows = await _connection.ExecuteAsync(sql, new { i_Id = role.Id });
                if (rows > 0)
                {
                    return IdentityResult.Success;
                }
                return IdentityResult.Failed(new IdentityError { Description = $"Could not delete role {role.RoleName}." });
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
                return IdentityResult.Failed(new IdentityError { Description = $"Could not delete role {role.RoleName}." });
                //throw;
            }
        }

        public async Task<RoleModel> FindByIdAsync(string roleId)
        {
            string sql = @"select Id,RoleName FROM useridentity.role where Id=@i_Id;";
            try
            {
                var result=  await _connection.QuerySingleAsync<RoleModel>(sql, new { i_Id = roleId});
                return result;
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
        public async Task<RoleModel> FindByNameAsync(string roleName)
        {
            string sql = @"select Id,RoleName FROM useridentity.role where RoleName=@i_RoleName;";
            try
            {
                var result = await _connection.QuerySingleAsync<RoleModel>(sql, new { i_Id = roleName });
                return result;
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
        }
    }
}
