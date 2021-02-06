namespace UserManagement.Domain.Enums
{
    public enum UserStatus
    {
        LoginFailed = 0,
        LoginSucceeded,
        LockedUser,
        MustChangePassword
    }
}
