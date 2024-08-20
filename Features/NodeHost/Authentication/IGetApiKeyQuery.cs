using System.Threading.Tasks;

namespace XOuranos.Features.NodeHost.Authentication
{
    public interface IGetApiKeyQuery
    {
        Task<ApiKey> Execute(string providedApiKey);
    }
}
