using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SysCandidato.Models.AccessBE
{
    public static class Access
    {

        public static object ExecuteScalar(IDbCommand cmd, bool registrolog = true)
        {
            int result = -1;
            int tentativa = 1;
            bool retry = false;
            bool transaction = true;

            do
            {
                if (retry && !transaction)
                    cmd.Connection = null;

                if (cmd.Connection == null)
                {
                    cmd.Connection = GetConnection();
                    transaction = false;
                }
                try
                {
                    foreach (MySqlParameter parmt in cmd.Parameters)
                        try
                        {
                            if (parmt.Value.ToString() == char.MinValue.ToString() || parmt.Value == null)
                                parmt.Value = DBNull.Value;
                        }
                        catch { };

                    if (cmd.Connection.State == ConnectionState.Closed)
                    {
                        transaction = false;
                        cmd.Connection.Open();
                    }

                    result = Convert.ToInt32(cmd.ExecuteScalar());

                    if (retry)
                        retry = false;
                    return result;
                }
                catch (MySqlException ex)
                {
                    result = -1;
                    //NetworkLog.Insert(ex, cmd.CommandText);
                    if (ex.Message.Contains("Exception while writing to stream") || ex.Message.Contains("Exception while reading from stream"))
                    {
                        if (!retry)
                            retry = true;
                        else
                        {
                            //MessageBox.Show("Atenção! Houve perda de conexão com o servidor ou ele demorou muito a responder." + (ex.InnerException != null ? "\r\n\r\nDetalhamento do erro: " + ex.InnerException.Message : "") + "\r\n\r\nÉ possível que as últimas alterações não foram salvas, por favor verifique.", "1) " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            retry = false;
                        }
                        //Application.Exit(new CancelEventArgs(true));
                    }
                    else if (ex.Data["Code"]?.ToString().CompareTo("08P01") == 0)
                    {
                        //MessageBox.Show("Atenção! Erro ao executar o comando:\r\n\r\n" + cmd.CommandText + "", "Violação de protocolo", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    }
                    else
                    {
                        TrataExcecao(ex, (MySqlCommand)cmd);
                    }

                }
                finally
                {
                    if (cmd.Connection.State == ConnectionState.Open)
                        cmd.Connection.Close();
                }
            } while (retry);
            return result;
        }

        public static int ExecuteNonQuery(IDbCommand cmd, bool registrolog = true)
        {
            int tentativa = 1;
            bool retry = false;
            bool transaction = true;
            int rowsAffected = 0;
            do
            {
                if (retry && !transaction)
                    cmd.Connection = null;

                if (cmd.Connection == null)
                {
                    cmd.Connection = GetConnection();
                    transaction = false;
                }
                try
                {
                    foreach (MySqlParameter parmt in cmd.Parameters)
                        try
                        {
                            if (parmt.Value == null || parmt.Value.ToString() == char.MinValue.ToString())
                                parmt.Value = DBNull.Value;
                        }
                        catch { };
                    if (cmd.Connection.State == ConnectionState.Closed)
                    {
                        transaction = false;
                        cmd.Connection.Open();
                    }

                    /*====================*/
                    rowsAffected = cmd.ExecuteNonQuery();

                    if (retry)
                        retry = false;
                }
                catch (MySqlException ex)
                {
                    //NetworkLog.Insert(ex, cmd.CommandText);
                    if (ex.Message.Contains("Exception while writing to stream") || ex.Message.Contains("Exception while reading from stream"))
                    {
                        if (!retry)
                            retry = true;
                        else
                        {
                            //MessageBox.Show("Atenção! Houve perda de conexão com o servidor ou ele demorou muito a responder." + (ex.InnerException != null ? "\r\n\r\nDetalhamento do erro: " + ex.InnerException.Message : "") + "\r\n\r\nÉ possível que as últimas alterações não foram salvas, por favor verifique.", "1) " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            retry = false;
                        }
                        //Application.Exit(new CancelEventArgs(true));
                    }
                    if (ex.Message.ToLower().Contains("unique key"))
                    {
                        string detalhes = "";

                        detalhes = ex.Message.Substring(ex.Message.Length / 2);

                        // MessageBox.Show("Violação ao inserir o registro!" +
                        //     "\n\r\n\r\n\r" +
                        //   "Informação duplicada não permitida em " + detalhes, "Atenção", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (ex.Data["Code"]?.ToString().CompareTo("08P01") == 0)
                    {
                        //MessageBox.Show("Atenção! Erro ao executar o comando:\r\n\r\n" + cmd.CommandText + "", "Violação de protocolo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        TrataExcecao(ex, (MySqlCommand)cmd);
                    }
                }
                finally
                {
                    if (cmd.Connection.State == ConnectionState.Open)
                        cmd.Connection.Close();
                }
            } while (retry);
            return rowsAffected;
        }

        public static IDbConnection GetConnection()
        {
            // string connectionString = "Server=" + Config.getInstance.IPServer + ";Port=" + Config.getInstance.Port + $";User ID=admin;Password=shift;Database=" + Config.getInstance.DBName + ";";//+ $";ApplicationName=MT-WINDOWS-{Environment.MachineName};";
            string connectionString = "Server=127.0.0.1;Database=candidatosbd;Uid=root;Pwd=;SSL Mode=none;Certificate Store Location=CurrentUser;";
            MySqlConnection conn = new MySqlConnection(connectionString);
            return conn;
        }

        //Verdadeiro CONHECIMENTO ABSORVIDO
        public static DataSet ExecuteReader(IDbCommand command)
        {
            int tentativa = 1;
            bool retry = false;
            DataSet ds = null;
            bool transaction = true;

            do
            {
                if (retry && !transaction)
                    command.Connection = null;

                if (command.Connection == null)
                {
                    command.Connection = GetConnection();
                    transaction = false;
                }

                try
                {
                    foreach (MySqlParameter parmt in command.Parameters)
                        try
                        {
                            if (parmt.Value == null || parmt.Value.ToString() == char.MinValue.ToString())
                                parmt.Value = DBNull.Value;
                        }
                        catch { };

                    if (command.Connection.State == ConnectionState.Closed)
                    {
                        command.Connection.Open();
                        transaction = false;
                    }

                    IDbDataAdapter da = new MySqlDataAdapter((MySqlCommand)command);
                    ds = new DataSet();

                    da.Fill(ds);

                    if (retry)
                        retry = false;
                }
                catch (MySqlException ex)
                {
                    //    NetworkLog.Insert(ex, command.CommandText);
                    if (ex.Message.Contains("Exception while writing to stream") || ex.Message.Contains("Exception while reading from stream"))
                    {
                        if (!retry)
                            retry = true;
                        else
                        {
                            //MessageBox.Show("Atenção! Houve perda de conexão com o servidor ou ele demorou muito a responder." + (ex.InnerException != null ? "\r\n\r\nDetalhamento do erro: " + ex.InnerException.Message : "") + "\r\n\r\nÉ possível que as últimas alterações não foram salvas, por favor verifique.", "4) " + ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            retry = false;
                        }
                    }

                    else if (ex.Data["Code"]?.ToString().CompareTo("08P01") == 0)
                    {
                        // MessageBox.Show("Atenção! Erro ao executar o comando:\r\n\r\n" + command.CommandText + "", "Violação de protocolo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        TrataExcecao(ex, (MySqlCommand)command);
                    }
                }

                finally
                {
                    if (command.Connection.State == ConnectionState.Open && !transaction)
                    {
                        command.Connection.Close();
                    }
                }
            } while (retry);
            return ds;
        }

        public static void TrataExcecao(MySqlException excecao, MySqlCommand Comando)
        {
            //  NetworkLog.Insert(excecao, Comando.CommandText);
            string detalhes = "";
            if (Comando.Parameters == null || Comando.Parameters.Count == 0)
                detalhes = "Comando: " + Comando.CommandText;
            else
            {
                detalhes = "Parâmetros:";
                foreach (MySqlParameter parmt in Comando.Parameters)
                    if (parmt.Value != null)
                        detalhes += parmt.Value.ToString() + ", ";
                detalhes = detalhes.Substring(0, detalhes.Length - 2) + ".";
            }
            switch (excecao.Data["Code"]?.ToString())
            {
                case "":
                    //   MessageBox.Show("Falha ao estabelecer conexão com o servidor (MRX157\\DEVELOPER).  O endereço IP do servidor está correto?\r\n* Porta 5432 no firewall do servidor está desbloqueada?\r\n* As configurações do servidor PostgreSQL estão corretas?", "Falha ao estabelecer conexão", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case "28000":
                    //  MessageBox.Show("Atenção! O nome do usuário e/ou a senha são inválidos. Tente novamente. ", "Erro ao entrar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case "25P02":
                    //  MessageBox.Show("Atenção! A transação atual foi abortada e os dados podem não ter sido salvos completamente. Motivo: " + excecao.Message + " \r\n\r\n" + detalhes, "Transação abortada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case "22P05":
                    //   MessageBox.Show("Atenção! Caractere inválido encontrado.\r\nErro:" + excecao.Message + "\r\n\r\n" + detalhes, "Erro ao inserir", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                case "28P01":
                    // MessageBox.Show("Atenção! O nome do usuário e/ou a senha são inválidos. Tente novamente.", "Erro ao autenticar", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                default:
                    //   MessageBox.Show(excecao.Data["Code"]?.ToString() + ": " + excecao.Message + "\r\n\r\n" + detalhes, "Erro no banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                    break;
            }
        }

        public static string Encrypt(string user, string pass)
        {
            string EncryptionKey = user + "Sh!ft";
            byte[] clearBytes = Encoding.Unicode.GetBytes(pass);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    pass = Convert.ToBase64String(ms.ToArray());
                }
            }
            return pass;
        }

        public static string Decrypt(string user, string pass)
        {
            try
            {
                string EncryptionKey = user + "Sh!ft";
                //pass = pass.Replace(" ", "+");
                byte[] cipherBytes = Convert.FromBase64String(pass);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);
                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        {
                            cs.Write(cipherBytes, 0, cipherBytes.Length);
                            cs.Close();
                        }
                        pass = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
            }
            catch (Exception ex)
            {
                // MessageBox.Show("Ocorreu um erro: " + ex.Message);
            }
            return pass;
        }
        
    }
}
