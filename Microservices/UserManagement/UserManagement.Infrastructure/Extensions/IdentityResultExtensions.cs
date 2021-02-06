using Microsoft.AspNetCore.Identity;
using UserManagement.Application.Common.Results;

namespace UserManagement.Infrastructure.Extensions
{
    public static class IdentityResultExtensions
    {
        public static Result ToApplicationResult(this Microsoft.AspNetCore.Identity.IdentityResult result)
        {
            return result.Succeeded
                ? Result.Success()
                : Result.Failure(string.Join("<br/>", result.Errors));
        }
    }
}
