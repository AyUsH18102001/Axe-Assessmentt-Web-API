using AxeAssessmentToolWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace AxeAssessmentToolWebAPI.Services
{
    public class SqlQuestionService : ISqlQuestionService
    {

        private readonly DataContext _dataContext;

        public async Task<IQueryable<SQL_Question>> GetAllSqlQuestions(int page)
        {
            if(page < 0)
            {
                return null;
            }
            IQueryable<SQL_Question> response;
            Pagination pagination = new Pagination();
            pagination.currentPage = page;
            pagination.pageSize = 3f;

            pagination.pageCount = (int) Math.Ceiling(_dataContext.SQL_Question.Count() / pagination.pageSize);

            response = _dataContext.SQL_Question.
                Skip((pagination.currentPage -1)* (int)pagination.pageSize).
                Take((int)pagination.pageSize);

            return response;
        }

        public async Task<int> GetPageCount()
        {
            Pagination pagination = new Pagination();
            pagination.pageSize = 3f;
            return (int)Math.Ceiling(_dataContext.SQL_Question.Count() / pagination.pageSize);
        }

        public SqlQuestionService(DataContext context)
        {
            this._dataContext = context;
        }

        public async Task<string> AddQuestion(SQL_Question question)
        {
            if (question == null)
            {
                return "Question is null";
            }
            await _dataContext.SQL_Question.AddAsync(question);
            await _dataContext.SaveChangesAsync();
            // add a record to Questions table to navigate to SQL_QuestionTable
            Question qu = new Question
            {
                QuestionContent = question.QuestionTitle,
                IsSQL = question.QuestionId,
                TestTypeId = 3,
            };
            await _dataContext.Questions.AddAsync(qu);
            await _dataContext.SaveChangesAsync();
            return "success";
        }

        public async Task<string> DeleteQuestion(int questionId)
        {
            // get question
            SQL_Question? question = await _dataContext.SQL_Question.FindAsync(questionId);
            if (question == null)
            {
                return "";
            }
            _dataContext.Remove(question);
            await _dataContext.SaveChangesAsync();
            return "success";
        }

        public async Task<SQL_Question> GetQuestion(int questionId)
        {
            SQL_Question? question = await _dataContext.SQL_Question.FindAsync(questionId);
            if (question == null)
            {
                return null;
            }
            return question;
        }

        public async Task<string> UpdateQuestion(int questionId, SQL_Question updatedQuestion)
        {
            // get question
            SQL_Question? question = await _dataContext.SQL_Question.FindAsync(questionId);
            if (question == null)
            {
                return "";
            }
            question.QuestionTitle = updatedQuestion.QuestionTitle;
            question.QuestionContent = updatedQuestion.QuestionContent;
            question.SQL_Answer = updatedQuestion.SQL_Answer;
            // Update the Question Title in the Question bank as well
            // get the question
            Question qu = await _dataContext.Questions.Where(q => q.IsSQL == questionId).FirstOrDefaultAsync();
            qu.QuestionContent = updatedQuestion.QuestionTitle;
            await _dataContext.SaveChangesAsync();
            return "success";
        }
    }
}
