using Microsoft.AspNetCore.Identity;
using UserManagement.Application.Common.Results;

namespace UserManagement.Infrastructure.Identity
{
    public static class IdentityResultExtensions
    {
        public static Result ToApplicationResult(this IdentityResult result)
        {
            return result.Succeeded
                ? Result.Success()
                : Result.Failure(string.Join("<br/>", result.Errors));
        }
    }
}
