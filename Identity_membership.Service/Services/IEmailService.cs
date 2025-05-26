namespace Identity_membership.Service.Services
{
    public interface IEmailService
    {
        Task SendResetPasswordEmail(string resetPasswordEmailLink, string To);
    }
}
