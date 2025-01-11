using Microsoft.AspNetCore.Authorization;

namespace Martiscoin.Features.NodeHost.Authorization
{
    public class OnlyAdminsRequirement : IAuthorizationRequirement
    {
    }
}
