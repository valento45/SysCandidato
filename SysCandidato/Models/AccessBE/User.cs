using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SysCandidato.Models.AccessBE
{
    public class User : IdentityUser
    {
        public string Id_Usuario { get; set; }
    
    }
}
