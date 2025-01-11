using System.Collections.Generic;
using Martiscoin.Features.NodeHost.Authentication;

namespace Martiscoin.Features.NodeHost.Settings
{
    public class MartiscoinSettings
    {
        public ApiKeys API { get; set; }
    }

    public class ApiKeys
    {
        public List<ApiKey> Keys { get; set; }
    }
}
