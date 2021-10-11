using Dapper;
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
    public class MyIdentityUserStore : UserStoreBase<User, string,
        IdentityUserClaim<string>, IdentityUserLogin<string>, IdentityUserToken<string>>
    {
        public MyIdentityUserStore(IdentityErrorDescriber describer) : base(describer)
        {
        }

        public override IQueryable<User> Users => throw new NotImplementedException();

        public override Task AddClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task AddLoginAsync(User user, UserLoginInfo login, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken = default)
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

        public override async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken = default)
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

        public override Task<User> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<IList<Claim>> GetClaimsAsync(User user, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<IList<UserLoginInfo>> GetLoginsAsync(User user, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<IList<User>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task RemoveClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task RemoveLoginAsync(User user, string loginProvider, string providerKey, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task ReplaceClaimAsync(User user, Claim claim, Claim newClaim, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        protected override Task AddUserTokenAsync(IdentityUserToken<string> token)
        {
            throw new NotImplementedException();
        }

        protected override Task<IdentityUserToken<string>> FindTokenAsync(User user, string loginProvider, string name, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        protected override Task<User> FindUserAsync(string userId, CancellationToken cancellationToken)
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
