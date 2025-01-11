using System.Threading.Tasks;

namespace Martiscoin.Features.NodeHost.Authentication
{
    public interface IGetApiKeyQuery
    {
        Task<ApiKey> Execute(string providedApiKey);
    }
}
