using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SysCandidato.Models.AccessBE
{
    public class User
    {
        public string Id_Usuario { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string PasswordHash { get; set; }
    }
}
