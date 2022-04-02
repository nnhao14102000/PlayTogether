using Microsoft.Extensions.Logging;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Interfaces.Services.Business;
using System;
using System.Threading.Tasks;

namespace PlayTogether.Core.Services.Business.Recommend
{
    public class RecommendService : IRecommendService
    {
        private readonly IRecommendRepository _recommendRepository;
        private readonly ILogger<RecommendService> _logger;

        public RecommendService(IRecommendRepository recommendRepository, ILogger<RecommendService> logger)
        {
            _recommendRepository = recommendRepository;
            _logger = logger;
        }
        public async Task<bool> WriteToFile()
        {
            try {
                return await _recommendRepository.WriteToFile();
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call TrainModel in service class, Error Message: {ex}.");
                throw;
            }
        }
    }
}