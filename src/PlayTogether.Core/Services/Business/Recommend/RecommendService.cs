using Microsoft.Extensions.Logging;
using PlayTogether.Core.Dtos.Outcoming.Generic;
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
        public async Task<Result<(double rootMeanSquaredError, double rSquared)>> TrainModel()
        {
            try {
                return await _recommendRepository.TrainModel();
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call TrainModel in service class, Error Message: {ex}.");
                throw;
            }
        }
    }
}