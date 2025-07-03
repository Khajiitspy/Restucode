namespace Core.Interface;

using Core.SMTP;

public interface ISmtpService
{
    Task<bool> SendEmailAsync(EmailMessage message);
}
