using System.Linq;

namespace AxeAssessmentToolWebAPI.Services
{
    public interface ISqlQuestionService
    {
        Task<IQueryable<SQL_Question>> GetAllSqlQuestions(int page);
        Task<int> GetPageCount();
        Task<string> AddQuestion(SQL_Question question);
        Task<string> DeleteQuestion(int questionId);
        Task<SQL_Question> GetQuestion(int questionId);
        Task<string> UpdateQuestion(int questionId,SQL_Question question);
    }
}
