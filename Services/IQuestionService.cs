namespace AxeAssessmentToolWebAPI.Services
{
    public interface IQuestionService
    {
        Task<List<Question>> GetQuestions();
        Task<List<Question>> AddQuestion(Question question);
        Task<List<Question>> DeleteQuestion(int id);
        Task<Question> EditQuestion(int id,Question updated);
    }
}
