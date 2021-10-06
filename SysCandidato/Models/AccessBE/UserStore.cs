using Dapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
namespace SysCandidato.Models.AccessBE
{
    public class UserStore : IUserStore<User>, IUserPasswordStore<User>
    {
        /// <summary>
        /// Insere o usuário no banco de dados
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
        {
            using (MySqlConnection con = Access.GetConnection() as MySqlConnection)
            {
                await con.ExecuteAsync("INSERT INTO users_tb (username, normalizedUserName, passwordhash) values (@username, @normalizedUserName, @passwordhash)",
                    new
                    {
                        userName = user.UserName,
                        normalizedUserName = user.NormalizedUserName,
                        passwordHash = user.PasswordHash
                    });
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
        {
            using (MySqlConnection con = Access.GetConnection() as MySqlConnection)
            {
                await con.ExecuteAsync("DELETE FROM users_tb WHERE id_usuario = @id",
                    new
                    {
                        id = user.Id_Usuario
                    });
            }

            return IdentityResult.Success;
        }

        public void Dispose()
        {
           
        }

        public async Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            using (MySqlConnection con = Access.GetConnection() as MySqlConnection)
            {
                if (con.State == System.Data.ConnectionState.Closed)
                    con.Open();
                return await con.QueryFirstOrDefaultAsync<User>("SELECT * FROM users_tb WHERE id_usuario = @id", new { id = userId });
            }
        }

        public async Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            using (MySqlConnection con = Access.GetConnection() as MySqlConnection)
            {
                if (con.State == System.Data.ConnectionState.Closed)
                    con.Open();
                return await con.QueryFirstOrDefaultAsync<User>("SELECT * FROM users_tb WHERE normalizedUserName = @name", new { name = normalizedUserName });
            }
        }

        public Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id_Usuario);
        }

        public Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
        {
            using (MySqlConnection con = Access.GetConnection() as MySqlConnection)
            {
                await con.ExecuteAsync("UPDATE users_tb SET username = @username, normalizedUserName = @normalizedUserName, passwordhash = @passwordhash WHERE id_usuario  = @id",
                    new
                    {
                        id = user.Id_Usuario,
                        username = user.UserName,
                        normalizedUserName = user.NormalizedUserName,
                        passwordhash = user.PasswordHash
                    });
            }
            return IdentityResult.Success;
        }

        public Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash != null); 
        }

        public Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }
    }
}
