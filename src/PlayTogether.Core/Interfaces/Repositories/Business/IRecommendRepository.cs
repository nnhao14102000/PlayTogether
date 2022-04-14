using PlayTogether.Core.Dtos.Outcoming.Generic;
using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Repositories.Business
{
    public interface IRecommendRepository
    {
        Task<Result<(double rootMeanSquaredError, double rSquared)>> TrainModel();
    }
}