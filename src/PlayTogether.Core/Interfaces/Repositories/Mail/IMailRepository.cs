using PlayTogether.Core.Dtos.Incoming.Mail;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Repositories.Mail
{
    public interface IMailRepository
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}