using MtAux.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SysCandidato.Models.MtCash
{
    public interface IConta : IRegistro
    {

        public string NomeFavorecido { get; set; }
        public string NomeDevedor { get; set; }

        public decimal Valor { get; set; }
        public DateTime DataVencimento { get; set; }
        public DateTime DataInicio { get; set; }
    }
}
