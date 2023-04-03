namespace AxeAssessmentToolWebAPI.Services
{
    public interface IEmailService
    {
        Task<string> SendTokenEmail(int usedId);
    }
}
