using System.Linq;
using System.Threading.Tasks;
using Martiscoin.Features.NodeHost.Settings;
using Microsoft.Extensions.Options;

namespace Martiscoin.Features.NodeHost.Authentication
{
    public class AppSettingsGetApiKeyQuery : IGetApiKeyQuery
    {
        private MartiscoinSettings settings;

        public AppSettingsGetApiKeyQuery(IOptionsMonitor<MartiscoinSettings> options)
        {
            this.settings = options.CurrentValue;

            // Make sure it is possible to edit the API keys while running.
            options.OnChange(config =>
            {
                this.settings = config;
            });
        }

        public Task<ApiKey> Execute(string providedApiKey)
        {
            ApiKey key = this.settings.API.Keys.Where(key => key.Key == providedApiKey && key.Enabled ==  true).SingleOrDefault();
            return Task.FromResult(key);
        }
    }
}
