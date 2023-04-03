namespace AxeAssessmentToolWebAPI.Services
{
    public interface IUserService
    {
        Task<List<User>> GetAllUser();
        Task<bool> RegisterUser(User user);
        Task<User> GetUser(int id);
        Task<string> GetUserId();
        Task<string> GetUserEmail();
        Task<string> UploadResume(int userId, IFormFile resume);
        Task<string> LoginUser(string email, string password);
        Task<string> LoginWithUserToken(string token);
        Task<bool> ForgotPassword(string email, string newPassword);
        Task<UserTest> GetUserTest(string email);
        Task<List<TestData>> GetTestData();
        Task<bool> IsUserTerminated(int userId);
        Task<int> GetUserViolation(int userId);
        Task<bool> UpdateUserViolation(int userId);
        Task<bool> UpdateScore(int userId,int questionId, List<int> options);
        Task<bool> UpdateEndTest(int userId);
    }
}
