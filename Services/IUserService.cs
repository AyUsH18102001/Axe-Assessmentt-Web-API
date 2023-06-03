using AxeAssessmentToolWebAPI.FrontendModel;
using AxeAssessmentToolWebAPI.Response_Models;

namespace AxeAssessmentToolWebAPI.Services
{
    public interface IUserService
    {
        Task<IQueryable<User>> GetAllUser(int page);
        Task<int> GetUserPageCount();
        Task<MessageAndCode> RegisterUser(User user);
        Task<User> GetUser(int id);
        Task<UserData> GetUserEmailAndId();
        Task<string> UploadResume(IFormFile resume);
        Task<string> GetUserResume(int userId);
        Task<string> UploadUserProfile(IFormFile image);
        Task<string> GetUserProfileImage(int userId);
        Task<Token> LoginUser(Login user);
        Task<string> LoginWithUserToken(string token);
        Task<string> UpdateQuestionAttempted(int userId,int questionId,List<int> optionIndex);
        Task<string> UserSubmitTest(TestSubmit answers);
        Task<string> ForgotPassword(string email, string newPassword);
        Task<UserTest> GetUserTest(string email);
        Task<List<TestData>> GetTestData(int userId);
        Task<List<CandidateRules>> GetTestRules(int userId);
        Task<IEnumerable<SQL_TestData>> SqlTestData(int userId);
        Task<CandidateTest_QuestionsData> GetTestQuestions(int userId);
        Task<bool> IsUserTerminated(int userId);
        Task<int> GetUserViolation(int userId);
        Task<bool> UpdateUserViolation(int userId);
        Task<bool> UpdateScore(int userId,int questionId, List<int> options);
        Task<bool> UpdateEndTest(int userId);
        Task<SQL_Response> RunSqlQuery(UserQuery query);
        Task<string> UpdateSqlScore(int userId);
        Task<string> SubmitQuery(List<SubmitSql> queries);
        Task<string> UserTestName(int userId);
        Task<TestInfo> GetTestInformation(int userId);
    }
}
