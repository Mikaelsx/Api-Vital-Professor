
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace WebAPI.Utils.Mail
{
    public class EmailService : IEmailService
    {
        // Armazena as configurações
        private readonly EmailSettings emailSettings;

        // Construtor que recebe as dependencias de Email settings
        public EmailService(IOptions<EmailSettings> options)
        {
            emailSettings = options.Value;
        }

        // Metodo de envio de email
        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            try
            {
                // Objeto que representa o email
                var email = new MimeMessage();

                // Define o remetente do email
                email.Sender = MailboxAddress.Parse(emailSettings.Email);

                // Destinatario do email
                email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));

                // Define o assunto do email
                email.Subject = mailRequest.Subject;

                // Cria o corpo do email
                var builder = new BodyBuilder();

                // Define o corpo do email como html
                builder.HtmlBody = mailRequest.Body;

                // define o corpo do email e do objeto MimeMessage
                email.Body = builder.ToMessageBody();

                // Cria um client smtp para envio de email
                using (var smtp = new SmtpClient())
                {
                    // Conecta-se ao servidor SMTP usando os dados de email settings
                    smtp.Connect(emailSettings.Host, emailSettings.Port, SecureSocketOptions.StartTls);

                    // Autentica-se no servidor SMTP usando os dados de email settings
                    smtp.Authenticate(emailSettings.Email, emailSettings.Password);

                    // Envia o email
                    await smtp.SendAsync(email);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
