using Microsoft.Extensions.Logging;
using PlayTogether.Core.Dtos.Incoming.Mail;
using PlayTogether.Core.Interfaces.Repositories.Mail;
using PlayTogether.Core.Interfaces.Services.Mail;
using System;
using System.Threading.Tasks;

namespace PlayTogether.Core.Services.Mail
{
    public class MailService : IMailService
    {
        private readonly IMailRepository _mailRepository;
        private readonly ILogger<MailService> _logger;

        public MailService(IMailRepository mailRepository, ILogger<MailService> logger)
        {
            _mailRepository = mailRepository;
            _logger = logger;
        }

        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            try {
                if (mailRequest is null) {
                    throw new ArgumentNullException(nameof(mailRequest));
                }
                await _mailRepository.SendEmailAsync(mailRequest);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call SendEmailAsync in service class, Error Message: {ex}.");
                throw;
            }
        }
    }
}