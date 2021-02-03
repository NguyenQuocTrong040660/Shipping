using System.Collections.Generic;

namespace UserManagement.Application.Common.Results
{
    public class LoginResult
    {
        public string UserName { get; set; }

        public IList<string> Roles { get; set; }

        public string OriginalUserName { get; set; }

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public bool IsLockedOut { get; set; }

        public bool Succeeded { get; set; }

        public string ErrorMessage { get; set; }

        public LoginResult() {}

        internal LoginResult(
           bool succeeded,
           string userName,
           IList<string> roles,
           string originalUserName,
           string accessToken,
           string refreshToken,
           string errorMessage)
        {
            Succeeded = succeeded;
            UserName = userName;
            Roles = roles;
            RefreshToken = refreshToken;
            AccessToken = accessToken;
            OriginalUserName = originalUserName;
            ErrorMessage = errorMessage;
        }

        internal LoginResult(
           bool succeeded,
           string error)
        {
            Succeeded = succeeded;
            ErrorMessage = error;
        }

        internal LoginResult(bool locked)
        {
            Succeeded = false;
            ErrorMessage = "User is currently locked out";
            IsLockedOut = locked;
        }

        public static LoginResult Success(
            string userName,
            IList<string> roles,
            string originalUserName,
            string accessToken,
            string refreshToken)
        {
            return new LoginResult(true,
                                    userName,
                                    roles,
                                    originalUserName,
                                    accessToken,
                                    refreshToken,
                                    string.Empty);
        }

        public static LoginResult Error(
            string error)
        {
            return new LoginResult(false, error);
        }

        public static LoginResult LockedOut()
        {
            return new LoginResult(locked: true);
        }
    }
}
