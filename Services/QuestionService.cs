using Microsoft.EntityFrameworkCore;

namespace AxeAssessmentToolWebAPI.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly DataContext _dataContext;
        public QuestionService(DataContext context) {
            this._dataContext = context;
        }

        public async Task<List<Question>> AddQuestion(Question question)
        {
            await _dataContext.Questions.AddAsync(question);
            await _dataContext.SaveChangesAsync();
            return await _dataContext.Questions.ToListAsync();
        }

        public async Task<List<Question>> DeleteQuestion(int id)
        {
            var question = await _dataContext.Questions.FindAsync(id);
            if (question is null)
            {
                return null;
            }
            _dataContext.Questions.Remove(question);
            await _dataContext.SaveChangesAsync();
            return await _dataContext.Questions.ToListAsync();
        }

        public async Task<List<Question>> GetQuestions()
        {
            return await _dataContext.Questions.ToListAsync();
        }

        public async Task<Question> EditQuestion(int id,Question updated)
        {
            var question = await _dataContext.Questions.FindAsync(id);
            if(question is null)
            {
                return null;
            }
            question.QuestionContent = updated.QuestionContent;
            question.QuestionsType = updated.QuestionsType;
            question.Options = updated.Options;

            await _dataContext.SaveChangesAsync();
            return question;
        }
    }
}
