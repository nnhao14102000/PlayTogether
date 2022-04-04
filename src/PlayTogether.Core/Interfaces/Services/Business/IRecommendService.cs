using System.Threading.Tasks;

namespace PlayTogether.Core.Interfaces.Services.Business
{
    public interface IRecommendService
    {
        Task<bool> WriteToFile();
    }
}