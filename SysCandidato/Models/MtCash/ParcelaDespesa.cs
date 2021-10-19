using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SysCandidato.Models.MtCash
{
    public class ParcelaDespesa : IConta
    {
        public string NomeFavorecido { get; set; }
        public string NomeDevedor { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataVencimento { get; set; }
        public DateTime DataInicio { get; set; }
        public int Id { get; set; }
        public string Modulo_ { get; set; }
        public string NomeReg { get; set; }
        public DateTime DataReg { get; set; }
        public int Id_Despesa { get; set; }


        private Despesa _despesa;
        public Despesa Despesa_
        {
            get
            {
                if(_despesa == null)
                {

                }
                return _despesa;
            }
            set
            {
                _despesa = value;
            }

        }
    }
}
