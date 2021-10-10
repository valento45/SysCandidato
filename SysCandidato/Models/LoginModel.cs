using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SysCandidato.Models
{
    public class LoginModel
    {

        public static LoginModel UserLogado = null;
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        private static int _HashCode;

        public static int GetHashCode()
        {
            return _HashCode;
        }

        public static void SetHashCode(string usuario)
        {
            _HashCode = usuario.GetHashCode();
        }

    }
}
