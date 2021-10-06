using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using SysCandidato.Models.AccessBE;
using System.Data;

namespace SysCandidato.Models
{
    public class VagasModel
    {
        public int IdVaga { get; set; }
        public string Descricao { get; set; }

        

        /// <summary>
        /// Retorna os candidatos cadastrados nessa vaga que foi instanciado usando técnica de Lazy loading
        /// </summary>
        public List<PessoasModel> Candidatos
        {
            get
            {
                if (_candidatos == null)
                {
                    if (IdVaga > 0)
                    {
                        _candidatos = GetCandidatosByIdVaga(IdVaga);
                    }
                    else
                        _candidatos = new List<PessoasModel>();
                }
                return _candidatos;
            }
            set
            {
                _candidatos = value;
            }
        }
        private List<PessoasModel> _candidatos;

        public VagasModel(DataRow dr)
        {
            int idvaga;
            int.TryParse(dr["id_vaga"].ToString(), out idvaga);
            IdVaga = idvaga;
            Descricao = dr["descricao"].ToString();
        }

        public static List<PessoasModel> GetCandidatosByIdVaga(int id)
        {
            List<PessoasModel> result = new List<PessoasModel>();
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM vaga_candidato_tb WHERE id_vaga = " + id);
            foreach (DataRow x in Access.ExecuteReader(cmd).Tables[0].Rows)
            {
                result.Add(new PessoasModel(x));
            }
            return result;
        }

        public static List<VagasModel> GetAllVagas()
        {
            List<VagasModel> result = new List<VagasModel>();
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM vagas_tb ;");
            foreach (DataRow x in Access.ExecuteReader(cmd).Tables[0].Rows)
            {
                result.Add(new VagasModel(x));
            }
            return result;
        }

        public static VagasModel GetVagaById(int id)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM vagas_tb where id_vaga = " + id);
            DataTable dt = Access.ExecuteReader(cmd).Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                return new VagasModel(dt.Rows[0]);
            }
            return null;
        }


        public static void InsertVaga(VagasModel obj)
        {
            MySqlCommand cmd = new MySqlCommand("INSERT INTO vagas_tb (descricao) VALUES (@descricao);");
            cmd.Parameters.AddWithValue(@"descricao", obj.Descricao);
            Access.ExecuteNonQuery(cmd);
        }

        public void InsertCandidatos()
        {
            foreach (var candidato in Candidatos)
            {
                PessoasModel.Insert(candidato);

                MySqlCommand cmd = new MySqlCommand("INSERT INTO vaga_candidato_tb (id_pessoa, id_vaga) VALUES (@id_pessoa, @id_vaga)");
                cmd.Parameters.AddWithValue(@"id_pessoa", candidato.IdPessoa);
                cmd.Parameters.AddWithValue(@"id_vaga", IdVaga);
                Access.ExecuteNonQuery(cmd);
            }
        }
    }
}
