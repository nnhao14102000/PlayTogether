using PlayTogether.Core.Dtos.Incoming.Mail;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Services.Mail
{
    public interface IMailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}