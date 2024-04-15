namespace WebAPI.Utils.Mail
{
    public interface IEmailService
    {
        // Metodo assincrono para envio de email
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
