namespace AxeAssessmentToolWebAPI.Services
{
    public interface IAdminService
    {
        Task<bool> AddAdminProfile(int userId);
    }
}
