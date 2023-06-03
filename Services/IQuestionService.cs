using AxeAssessmentToolWebAPI.FrontendModel;

namespace AxeAssessmentToolWebAPI.Services
{
    public interface IQuestionService
    {
        Task<IQueryable<Question>> GetQuestions(int currentPage);
        Task<IQueryable<SQL_Question>> GetSQL_Questions(int currentPage);
        Task<int> GetSqlQuestionsPageCount();
        Task<bool> DeleteSqlQuestion(int questionId,int userId);
        Task<string> AddBulkQuestions(List<Question> questions);
        Task<int> GetQuestionPageCount();
        Task<Question> GetQuestionByID(int id);
        Task<SQL_TestData> GetSqlQuestionById(int id);
        Task<List<QuestionType>> GetQuestionTypes();
        Task<string> AddQuestion(Question question);
        Task<string> AddQuestionImage(IFormFile? qImage);
        Task<string> GetQuestionImage(int questionId);
        Task<string> DeleteQuestion(PatchQuestion question);
        Task<string> EditQuestion(int id,Question updated);
        Task<Question> LastQuestion();
        Task<string> GetQuestionType(int questionTypeId);
        Task<List<Option>> GetQuestionOptions(int questionId);
        Task<List<TestType>> GetAllTestTypes();
        Task<List<Question>> Get_TestType_Questions(int testType);
        Task<List<Question>> Get_QuestionType_Questions(int questionType);
    }
}
