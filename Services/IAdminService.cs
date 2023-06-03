using AxeAssessmentToolWebAPI.Response_Models;

namespace AxeAssessmentToolWebAPI.Services
{
    public interface IAdminService
    {
        Task<bool> AddAdminProfile(int userId);
        Task<bool> UpdateCandidateSelction(int userId, bool status);
        Task<bool> AddBulkQuestionsData(IFormFile file);
        Task<string> DownloadTestFormat();
        Task<string> GetCandidateProfile(int userId);
        Task<List<Rules>> GetAllTestRules();
        Task<string> CreateTest(Test test);
        Task<string> DeleteTest(int testId,int deleteId);
        Task<IQueryable<TestDetails>> GetAllTest();
        Task<string> UpdateUserTestId(int testId,int userId);
        Task<string> GetTestCreator(int creatorId);
        Task<bool> UpateSelectionState(int userId, int state);
        Task<List<TestPaper>> GetTestPaperQuestions(int testId);
        Task<string> UpdateTest(int testId, Test updated);
        Task<List<TableDefinition>> GetTableSchemas();
        Task<IQueryable<Rules>> GetTestRules(int userId);
        Task<IQueryable<Rules>> GetTestRules_Edit(int userId);
    }
}
