using PlayTogether.Core.Dtos.Outcoming.Generic;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Services.Business
{
    public interface IRecommendService
    {
        Task<Result<(double rootMeanSquaredError, double rSquared)>> TrainModel();
    }
}