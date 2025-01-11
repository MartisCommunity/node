using Microsoft.AspNetCore.Mvc;

namespace Martiscoin.Features.NodeHost.Authorization
{
    public class ForbiddenProblemDetails : ProblemDetails
    {
        public ForbiddenProblemDetails(string details = null)
        {
            this.Title = "Forbidden";
            this.Detail = details;
            this.Status = 403;
            this.Type = "https://httpstatuses.com/403";
        }
    }
}
