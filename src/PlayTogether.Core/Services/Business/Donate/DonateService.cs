using Microsoft.Extensions.Logging;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Interfaces.Services.Business;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PlayTogether.Core.Services.Business.Donate
{
    public class DonateService : IDonateService
    {
        private readonly IDonateRepository _donateRepository;
        private readonly ILogger<DonateService> _logger;

        public DonateService(IDonateRepository donateRepository, ILogger<DonateService> logger)
        {
            _donateRepository = donateRepository;
            _logger = logger;
        }

        
    }
}