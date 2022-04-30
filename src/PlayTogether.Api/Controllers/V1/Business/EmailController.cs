using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayTogether.Core.Dtos.Incoming.Auth;
using PlayTogether.Core.Dtos.Incoming.Mail;
using PlayTogether.Core.Interfaces.Services.Mail;
using System.Threading.Tasks;

namespace PlayTogether.Api.Controllers.V1.Business
{
    [ApiVersion("1.0")]
    public class EmailController : BaseController
    {
        private readonly IMailService _mailService;
        public EmailController(IMailService mailService)
        {
            _mailService = mailService;
        }

        /// <summary>
        /// Send email
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <remarks>
        /// Roles Access: Admin, User, Charity
        /// </remarks>
        [HttpPost("send")]
        [Authorize(Roles = AuthConstant.RoleAdmin + "," + AuthConstant.RoleUser + "," + AuthConstant.RoleCharity)]
        public async Task<ActionResult> Send(MailRequest request)
        {
            await _mailService.SendEmailAsync(request);
            return Ok();
        }
    }
}