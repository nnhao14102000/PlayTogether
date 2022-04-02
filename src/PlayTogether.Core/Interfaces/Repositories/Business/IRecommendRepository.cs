using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Repositories.Business
{
    public interface IRecommendRepository
    {
        Task<bool> TrainModel();
    }
}