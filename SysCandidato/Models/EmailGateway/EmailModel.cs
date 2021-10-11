using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SysCandidato.Models.EmailGateway
{
    public class EmailModel 
    {
        /*
      1º O remetente deve ser de um email com domínio o gmail.com ( conta google ), pois foi programado utilizando classes gratuitas (existem pagas ), no entanto, o destinatario pode possuir qualquer dominio, apenas quem envia deve possuir dominio gmail.com.
      2º O responsável pelo envio deve acessar esse link e permitir o acesso de aplicativos menos seguro (nosso software ): https://myaccount.google.com/lesssecureapps
          */


        /// <summary>
        /// Transmite uma mensagem de email sem anexos
        /// </summary>
        /// <param name="Destinatario">Destinatario (Recipient)</param>
        /// <param name="Remetente">Remetente (Sender)</param>
        /// <param name="Assunto">Assunto da mensagem (Subject)</param>
        /// <param name="enviaMensagem">Corpo da mensagem(Body)</param>
        /// <returns>Status da mensagem</returns>
        public static string EnviaMensagemEmail(string destinatario, string remetente, string assunto, string enviaMensagem, string email, string senha)
        {
            try
            {
                bool validaEmail = ValidaEnderecoEmail(destinatario);
                if (!validaEmail)
                    return "Email do destinatário é inválido: " + destinatario;
                //cria uma mensagem
                MailMessage mensagemEmail = new MailMessage(remetente, destinatario, assunto, enviaMensagem);

                //----------------------------------------------------------------------------------------------------------------------------------
                //obtem os valores smtp do arquivo de configuração . Não vou usar estes valores estou apenas mostrando como obtê-los
                //Configuration configurationFile = WebConfigurationManager.OpenWebConfiguration(null);
                //MailSettingsSectionGroup mailSettings = configurationFile.GetSectionGroup("system.net/mailSettings") as MailSettingsSectionGroup;
                //if (mailSettings != null)
                //{
                //    string host = mailSettings.Smtp.Network.Host;
                //    string password = mailSettings.Smtp.Network.Password;
                //    string username = mailSettings.Smtp.Network.UserName;
                //    int port = mailSettings.Smtp.Network.Port;
                //}
                //--

                //configuracoes do smtp
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Host = "smtp.gmail.com";
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = true;
                smtpClient.Credentials = new NetworkCredential(email, senha);

                //envia mensagem 
                smtpClient.Send(mensagemEmail);

                return "Mensagem enviada para " + destinatario + " às " + DateTime.Now.ToString() + ".";
            }
            catch (SmtpException ex)
            {
                return ex.Message;
            }
        }


        /// <summary>
        /// Transmite uma mensagem de email com anexos
        /// </summary>
        /// <param name="Destinatario">Destinatario (Recipient)</param>
        /// <param name="Remetente">Remetente (Sender)</param>
        /// <param name="Assunto">Assunto da mensagem (Subject)</param>
        /// <param name="enviaMensagem">Corpo da mensagem(Body)</param>
        /// <returns>Status da mensagem</returns>
        public static string EnviaMensagemEmailComAnexo(string destinatario, string remetente, string assunto, string enviaMensagem, string email, string senha, string caminhoAnexo)
        {
            try
            {
                bool validaEmail = ValidaEnderecoEmail(destinatario);
                if (!validaEmail)
                    return "Email do destinatário é inválido: " + destinatario;
                //cria uma mensagem
                MailMessage mensagemEmail = new MailMessage(remetente, destinatario, assunto, enviaMensagem);

                if (caminhoAnexo.Length > 0)
                {
                    Attachment anexado = new Attachment(caminhoAnexo, MediaTypeNames.Application.Octet);
                    mensagemEmail.Attachments.Add(anexado);
                }
                //configuracoes do smtp
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Host = "smtp.gmail.com";
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.UseDefaultCredentials = true;
                smtpClient.Credentials = new NetworkCredential(email, senha);

                //envia mensagem 
                smtpClient.Send(mensagemEmail);

                return "Mensagem enviada para " + destinatario + " às " + DateTime.Now.ToString() + ".";
            }
            catch (SmtpException ex)
            {
                return ex.Message;
            }

        }


        /// <summary>
        /// Confirma a validade de um email
        /// </summary>
        /// <param name="enderecoEmail">Email a ser validado</param>
        /// <returns>Retorna True se o email for valido</returns>
        public static bool ValidaEnderecoEmail(string enderecoEmail)
        {
            try
            {
                //define a expressão regulara para validar o email
                string texto_Validar = enderecoEmail;
                Regex expressaoRegex = new Regex(@"\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}");

                // testa o email com a expressão
                if (expressaoRegex.IsMatch(texto_Validar))
                {
                    // o email é valido
                    return true;
                }
                else
                {
                    // o email é inválido
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
