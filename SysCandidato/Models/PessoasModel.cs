using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using SysCandidato.Models.AccessBE;

namespace SysCandidato.Models
{
    public class PessoasModel
    {
        public int IdPessoa { get; set; }
        public int IdVaga { get; set; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }
        public int CPF { get; set; }
        public DateTime DataNascimento { get; set; }

        public PessoasModel() { }
        public PessoasModel(DataRow dr)
        {
            int idpessoa;
            int.TryParse(dr["id_pessoa"].ToString(), out idpessoa);
            IdPessoa = idpessoa;
            Nome = dr["nome"].ToString();
            Sobrenome = dr["sobrenome"].ToString();
            int cpf;
            int.TryParse(dr["cpf"].ToString(), out cpf);
            CPF = cpf;
            DataNascimento = dr["data_nascimento"] != null ? DateTime.Parse(dr["data_nascimento"].ToString()) : new DateTime();
        }


        /// <summary>
        /// Insere pessoa no banco de dados.
        /// </summary>
        /// <param name="obj"></param>
        public static void Insert(PessoasModel obj)
        {
            MySqlCommand cmd = new MySqlCommand("INSERT INTO pessoa_tb (nome, sobrenome, cpf, data_nascimento) VALUES (@nome, @sobrenome, @cpf, @data_nascimento); SELECT LAST_INSERT_ID();");
            cmd.Parameters.AddWithValue(@"nome", obj.Nome);
            cmd.Parameters.AddWithValue(@"sobrenome", obj.Sobrenome);
            cmd.Parameters.AddWithValue(@"cpf", obj.CPF);
            cmd.Parameters.AddWithValue(@"data_nascimento", obj.DataNascimento);

            int id;
            int.TryParse( Access.ExecuteScalar(cmd).ToString(), out id);
            obj.IdPessoa = id;
        }
    }
}
