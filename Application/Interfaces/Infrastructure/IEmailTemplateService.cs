namespace Application.Interfaces.Infrastructure;

public interface IEmailTemplateService
{
    void SendConfirmationCode(ApplicationUser user, string code, int expiryTimeInMinutes);
    void SendResetPassword(ApplicationUser user, string code, int expiryTimeInHours = 24);
    void SendChangeEmailNotification(ApplicationUser user, string oldEmail, DateTime changeDate);
}
