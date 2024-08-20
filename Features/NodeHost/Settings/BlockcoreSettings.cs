using System.Collections.Generic;
using XOuranos.Features.NodeHost.Authentication;

namespace XOuranos.Features.NodeHost.Settings
{
    public class XOuranosSettings
    {
        public ApiKeys API { get; set; }
    }

    public class ApiKeys
    {
        public List<ApiKey> Keys { get; set; }
    }
}
