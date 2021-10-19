using Dapper;
using ExtensionMethods.ConnectionsGateway;
using Microsoft.AspNetCore.Identity;
using MtAux.Interfaces;
using MySql.Data.MySqlClient;
using SysCandidato.Models.AccessBE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SysCandidato.Models.MtCash
{
    public class Despesa : IConta
    {
        public string NomeFavorecido { get; set; }
        public string NomeDevedor { get; set; }
        public decimal Valor { get; set; }
        public int Id { get; set; }
        public string Modulo_ { get; set; }
        public string NomeReg { get; set; }
        public DateTime DataReg { get; set; }
        public DateTime DataVencimento { get; set; }
        public DateTime DataInicio { get; set; }


        public async Task<IdentityResult> CreateAsync()
        {
            using (MySqlConnection con = Access.GetConnection().ToMySql())
            {
                await con.ExecuteAsync("INSERT INTO mt.despesa_tb (nomeFavorecido, nomeDevedor, valor, modulo, nomeRegistro, dataRegistro, dataInicio, dataVencimento) VALUES" +
                    " (@nomeFavorecido, @nomeDevedor, @valor, @modulo, @nomeRegistro, @dataRegistro, @dataInicio, @DataVencimento)",
                    new
                    {
                        nomeFavorecido = NomeFavorecido,
                        nomeDevedor = NomeDevedor,
                        valor = Valor,
                        modulo = Modulo_,
                        nomeRegistro = NomeReg,
                        dataRegistro = DataReg,
                        dataInicio = DataInicio,
                        dataVencimento = DataVencimento
                    });
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> UpdateAsync()
        {
            using (MySqlConnection con = Access.GetConnection().ToMySql())
            {
                await con.ExecuteAsync("UPDATE mt.despesa_tb SET nomeFavorecido = @nomeFavorecido, nomeDevedor = @nomeDevedor," +
                    " valor = @valor, modulo = @modulo, nomeRegistro = @nomeRegistro, dataRegistro = @dataRegistro, dataInicio = @dataInicio, " +
                    "dataVencimento = @dataVencimento WHERE idDespesa = @idDespesa;",
                    new
                    {
                        idDespesa = Id,
                        nomeFavorecido = NomeFavorecido,
                        nomeDevedor = NomeDevedor,
                        valor = Valor,
                        modulo = Modulo_,
                        nomeRegistro = NomeReg,
                        dataRegistro = DataReg,
                        dataInicio = DataInicio,
                        dataVencimento = DataVencimento
                    });
            }

            return IdentityResult.Success;
        }

        public async Task<IdentityResult> Delete()
        {
            using (MySqlConnection con = Access.GetConnection().ToMySql())
            {
                await con.ExecuteAsync("DELETE mt.despesa_tb WHERE idDespesa = @idDespesa;",
                    new
                    {
                        idDespesa = Id
                    });
            }

            return IdentityResult.Success;
        }

        public async Task<IEnumerable<Despesa>> GetAll(int limit = 0)
        {
            using (MySqlConnection con = Access.GetConnection().ToMySql())
            {
                if (con.State == System.Data.ConnectionState.Closed)
                    con.Open();
                return await con.QueryAsync<Despesa>("SELECT * FROM mt_despesa_tb" + (limit > 0 ? $" LIMIT {limit};" : ";"));
            }
        }
    }
}
