using Dapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using ExtensionMethods.ConnectionsGateway;

namespace SysCandidato.Models.AccessBE
{
    public class UserStore : IUserStore<Usuario>, IUserPasswordStore<Usuario>, IUserEmailStore<Usuario>, IUserLockoutStore<Usuario>
    {
        /// <summary>
        /// Insere o usuário no banco de dados
        /// </summary>
        /// <param name="user"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IdentityResult> CreateAsync(Usuario user, CancellationToken cancellationToken)
        {
            using (MySqlConnection con = Access.GetConnection().ToMySql())
            {
                await con.ExecuteAsync("INSERT INTO users_tb (username, normalizedUserName, passwordhash, email, confirmedEmail) values (@username, @normalizedUserName, @passwordhash, @email, @confirmedEmail)",
                    new
                    {
                        userName = user.UserName,
                        normalizedUserName = user.NormalizedUserName,
                        passwordHash = user.PasswordHash,
                        email = user.Email,
                        confirmedEmail = user.EmailConfirmed
                        
                    }); 
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(Usuario user, CancellationToken cancellationToken)
        {
            using (MySqlConnection con = Access.GetConnection().ToMySql())
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

        public async Task<Usuario> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            using (MySqlConnection con = Access.GetConnection().ToMySql())
            {
                if (con.State == System.Data.ConnectionState.Closed)
                    con.Open();
                return await con.QueryFirstOrDefaultAsync<Usuario>("SELECT * FROM users_tb WHERE id_usuario = @id", new { id = userId });
            }
        }

        public async Task<Usuario> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            using (MySqlConnection con = Access.GetConnection().ToMySql())
            {
                if (con.State == System.Data.ConnectionState.Closed)
                    con.Open();
                return await con.QueryFirstOrDefaultAsync<Usuario>("SELECT * FROM users_tb WHERE normalizedUserName = @name", new { name = normalizedUserName });
            }
        }

        public Task<string> GetNormalizedUserNameAsync(Usuario user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetUserIdAsync(Usuario user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id_Usuario);
        }

        public Task<string> GetUserNameAsync(Usuario user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(Usuario user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(Usuario user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(Usuario user, CancellationToken cancellationToken)
        {
            using (MySqlConnection con = Access.GetConnection().ToMySql())
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

        public Task<bool> HasPasswordAsync(Usuario user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash != null);
        }

        public Task SetPasswordHashAsync(Usuario user, string passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public Task<string> GetPasswordHashAsync(Usuario user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task SetEmailAsync(Usuario user, string email, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetEmailAsync(Usuario user, CancellationToken cancellationToken)
        {
            using (var connection = Access.GetConnection().ToMySql())
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                    connection.Open();

                return await connection.QueryFirstOrDefaultAsync<string>("Select email From users_tb WHERE username = @username", new
                {
                    username = user.UserName
                });
            }
        }

        public async Task<bool> GetEmailConfirmedAsync(Usuario user, CancellationToken cancellationToken)
        {
            using (var connection = Access.GetConnection().ToMySql())
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                    connection.Open();

                return await connection.QueryFirstOrDefaultAsync<bool>("Select confirmedEmail From users_tb WHERE username = @username", new
                {
                    username = user.UserName
                });
            }
        }

        public Task SetEmailConfirmedAsync(Usuario user, bool confirmed, CancellationToken cancellationToken)
        {
            using (var connection = Access.GetConnection().ToMySql())
            {
                connection.ExecuteAsync("UPDATE users_tb SET confirmedEmail = @confirmedEmail", new
                {
                    confirmedEmail = true
                });
                return Task.CompletedTask;
            }

        }

        public async Task<Usuario> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            using (MySqlConnection con = Access.GetConnection().ToMySql())
            {
                if (con.State == System.Data.ConnectionState.Closed)
                    con.Open();
                return await con.QueryFirstOrDefaultAsync<Usuario>("SELECT * FROM users_tb WHERE email = @name", new { name = normalizedEmail });
            }
        }
        

        public Task<string> GetNormalizedEmailAsync(Usuario user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetNormalizedEmailAsync(Usuario user, string normalizedEmail, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public Task<DateTimeOffset?> GetLockoutEndDateAsync(Usuario user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task SetLockoutEndDateAsync(Usuario user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<int> IncrementAccessFailedCountAsync(Usuario user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task ResetAccessFailedCountAsync(Usuario user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetAccessFailedCountAsync(Usuario user, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<bool> GetLockoutEnabledAsync(Usuario user, CancellationToken cancellationToken)
        {


            throw new NotImplementedException();
        }

        public Task SetLockoutEnabledAsync(Usuario user, bool enabled, CancellationToken cancellationToken)
        {
            user.LockoutEnabled = enabled;
            return Task.CompletedTask;
        }
    }
}
