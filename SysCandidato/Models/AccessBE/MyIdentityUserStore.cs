using Dapper;
using ExtensionMethods.ConnectionsGateway;
using Microsoft.AspNetCore.Identity;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace SysCandidato.Models.AccessBE
{
    public class MyIdentityUserStore : UserStoreBase<Usuario, string,
        IdentityUserClaim<string>, IdentityUserLogin<string>, IdentityUserToken<string>>
    {
        public MyIdentityUserStore(IdentityErrorDescriber describer) : base(describer)
        {
        }

        public override IQueryable<Usuario> Users => throw new NotImplementedException();

        public override Task AddClaimsAsync(Usuario user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task AddLoginAsync(Usuario user, UserLoginInfo login, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override async Task<IdentityResult> CreateAsync(Usuario user, CancellationToken cancellationToken = default)
        {
            using (MySqlConnection con = Access.GetConnection() as MySqlConnection)
            {
                await con.ExecuteAsync("INSERT INTO users_tb (username, normalizedUserName, passwordHash, email) VALUES (@username, @normalizedUserName, @passwordHash, @email)",
                    new
                    {
                        user.UserName,
                        user.NormalizedUserName,
                        user.PasswordHash,
                        user.Email
                    });
            }
            return IdentityResult.Success;
        }

        public override async Task<IdentityResult> DeleteAsync(Usuario user, CancellationToken cancellationToken = default)
        {
            using (MySqlConnection con = Access.GetConnection() as MySqlConnection)
            {
                await con.ExecuteAsync("DELETE FROM users_tb WHERE id_usuario = @id_usuario;",
                    new
                    {
                        user.Id_Usuario
                    });
            }
            return IdentityResult.Success;
        }

        public override async Task<Usuario> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default)
        {
            using (var connection = Access.GetConnection().ToMySql())
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                    connection.Open();

                return  await connection.QueryFirstOrDefaultAsync<Usuario>("Select * From users_tb WHERE email = @email", new
                {
                    email = normalizedEmail
                });

            }
        }

        public override Task<Usuario> FindByIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override async Task<Usuario> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = default)
        {
            using (var connection = Access.GetConnection().ToMySql())
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                    connection.Open();

                return await connection.QueryFirstOrDefaultAsync<Usuario>("Select * From users_tb WHERE username = @username", new
                {
                    username = normalizedUserName
                });
            }
        }

        public override Task<IList<Claim>> GetClaimsAsync(Usuario user, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<IList<UserLoginInfo>> GetLoginsAsync(Usuario user, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<IList<Usuario>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task RemoveClaimsAsync(Usuario user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task RemoveLoginAsync(Usuario user, string loginProvider, string providerKey, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task ReplaceClaimAsync(Usuario user, Claim claim, Claim newClaim, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<IdentityResult> UpdateAsync(Usuario user, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        protected override Task AddUserTokenAsync(IdentityUserToken<string> token)
        {
            throw new NotImplementedException();
        }

        protected override Task<IdentityUserToken<string>> FindTokenAsync(Usuario user, string loginProvider, string name, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        protected override Task<Usuario> FindUserAsync(string userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        protected override Task<IdentityUserLogin<string>> FindUserLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        protected override Task<IdentityUserLogin<string>> FindUserLoginAsync(string userId, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        protected override Task RemoveUserTokenAsync(IdentityUserToken<string> token)
        {
            throw new NotImplementedException();
        }
    }
}
