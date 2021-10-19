using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MtAux.Interfaces
{
    public interface IRegistro
    {
        public int Id { get; set; }
        public string Modulo_ { get; set; }
        public string NomeReg { get; set; }
        public DateTime DataReg { get; set; }


        public int GetId() { return Id; }
        public string Modulo() { return Modulo_; }
        public DateTime DataRegistro() { return DataReg; }
        public string NomeRegistro() { return NomeReg; }

    }
}
