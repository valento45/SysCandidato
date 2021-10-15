using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SysCandidato.Models.AccessBE
{
    public class ValidadorSenha<TUser> : IPasswordValidator<TUser> where TUser : class
    {
        public async Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user, string password)
        {
            var username = await manager.GetUserNameAsync(user);

            if(username == password)
            {
                return IdentityResult.Failed(new IdentityError { Description= "A senha não pode ser igual ao usuário."});
            }
            if(password.ToLower() == "password")
            {
                return IdentityResult.Failed(new IdentityError { Description = "A senha não pode ser exatamente 'password'." });
            }
            if(password.ToLower() == "senha")
            {
                return IdentityResult.Failed(new IdentityError { Description = "A senha não pode ser exatamente 'senha'." });
            }
            return IdentityResult.Success;

        }
    }  
}
