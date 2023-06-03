using AxeAssessmentToolWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using AxeAssessmentToolWebAPI.FrontendModel;
using AxeAssessmentToolWebAPI.Migrations;
using Azure;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AxeAssessmentToolWebAPI.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly DataContext _dataContext;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHostingEnvironment _hostEnvironment;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public QuestionService(DataContext context, IConfiguration configuration, IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, IHostingEnvironment hostEnvironment)
        {
            this._dataContext = context;
            this._configuration = configuration;
            this._webHostEnvironment = webHostEnvironment;
            this._httpContextAccessor = httpContextAccessor;
            this._hostEnvironment = hostEnvironment;
        }

        public async Task<string> AddQuestion(Question question)
        {
            string response = "success";
            // check if question already exists
            Question? qu = await _dataContext.Questions.Where(q => q.QuestionContent == question.QuestionContent).FirstOrDefaultAsync();
            if (qu == null)
            {

                await _dataContext.Questions.AddAsync(question);
                await _dataContext.SaveChangesAsync();
            }
            if (qu != null)
            {
                response = "You cannot add duplicate questions";
            }
            return response;
        }

        public async Task<Question> LastQuestion()
        {
            return await _dataContext.Questions.LastAsync();
        }

        public async Task<string> AddQuestionImage(IFormFile? qImage)
        {
            string appRoot = this._hostEnvironment.ContentRootPath;
            string app_data_path = Path.Combine(appRoot, "wwwroot");

            var localFileName = qImage.FileName;

            var filePath = Path.Combine(app_data_path, "QuestionImages", localFileName);
            var stream = new FileStream(filePath.Trim('"'), FileMode.Create);
            qImage.CopyTo(stream);
            stream.Close();
            return "success";
        }

        public async Task<string> GetQuestionImage(int questionId)
        {
            string hostURL = "https://localhost:7143";
            string? localName = _dataContext.Questions.FindAsync(questionId).Result.QuestionImage;
            if (localName == null)
            {
                return "";
            }
            else
            {
                return hostURL + "/QuestionImages/" + localName;
            }   
            /*
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
            // get the image localname
            string? localName = _dataContext.Questions.FindAsync(questionId).Result.QuestionImage;

            // set the directry path
            string appRoot = this._hostEnvironment.ContentRootPath;
            string app_data_path = Path.Combine(appRoot, "wwwroot");

            var filePath = Path.Combine(app_data_path, "QuestionImages", localName);

            // get the extension of image for setting the mime
            var ext = Path.GetExtension(filePath);

            var contents = File.ReadAllBytes(filePath);

            // create the memory stream
            MemoryStream ms = new MemoryStream(contents);
            response.Content = new StreamContent(ms);
            response.StatusCode = HttpStatusCode.OK;
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("image/" + ext);
            ms.Close();
            return response;
            */
        }

        public async Task<Question> GetQuestionByID(int id)
        {
            Question question = await _dataContext.Questions.FindAsync(id);
            return question == null ? null : question;
        }

        public async Task<SQL_TestData> GetSqlQuestionById(int id)
        {
            SQL_Question question = await _dataContext.SQL_Question.FindAsync(id);
            return new SQL_TestData
            {
                QuestionId = question.QuestionId,
                QuestionTitle = question.QuestionTitle,
            };
        }

        public async Task<string> DeleteQuestion(PatchQuestion patch)
        {
            var question = await _dataContext.Questions.FindAsync(patch.questionId);
            if (question is null)
            {
                return null;
            }
            // Performing soft delete
            question.D_Date = patch.deleteDate;
            question.D_Id = patch.adminId;
            await _dataContext.SaveChangesAsync();
            return "success";
        }

        public async Task<int> GetQuestionPageCount()
        {
            Pagination page = new Pagination();
            page.pageCount = (int)Math.Ceiling(_dataContext.Questions.Where(q => q.D_Date == null).Count() / page.pageSize);
            return page.pageCount;
        }

        public async Task<IQueryable<Question>> GetQuestions(int currentPage)
        {
            if (currentPage < 0)
            {
                return null;
            }
            IQueryable<Question> response;
            Pagination page = new Pagination();
            page.currentPage = currentPage;
            page.pageCount = (int) Math.Ceiling(_dataContext.Questions.Count() / page.pageSize);

            response = _dataContext.Questions.
                Where(q => q.D_Date == null && (q.IsSQL == 0 || q.IsSQL == null)).
                Skip((currentPage - 1) * (int)page.pageSize).
                Take((int)page.pageSize);
            return response;
        }

        public async Task<List<Option>> GetQuestionOptions(int questionId)
        {
            // get the question
            Question question = await _dataContext.Questions.FindAsync(questionId);
            return question != null ? question.Options : new List<Option>();
        }

        public async Task<string> AddBulkQuestions(List<Question> questions)
        {
            string response = "success";
            foreach(Question question in questions)
            {
                // check if question already exists
                Question? qu = await _dataContext.Questions.Where(q => q.QuestionContent == question.QuestionContent).FirstOrDefaultAsync();
                if(qu == null)
                {

                    await _dataContext.Questions.AddAsync(question);
                    await _dataContext.SaveChangesAsync();
                }
                if (qu != null)
                {
                    
                    response = "You cannot add duplicate questions";
                }
            }
            return response;
        }

        public async Task<int> GetSqlQuestionsPageCount()
        {
            Pagination page = new Pagination();
            IQueryable<SQL_Question> sqlQuestions = (from sql in _dataContext.SQL_Question
                                                     join qu in _dataContext.Questions
                                                     on sql.QuestionId equals qu.IsSQL
                                                     where (qu.D_Date == null)
                                                     select sql);
            return (int)Math.Ceiling(sqlQuestions.Count() / page.pageSize);
        }

        public async Task<bool> DeleteSqlQuestion(int questionId, int userId)
        {
            // get the Question
            Question question = await _dataContext.Questions.Where(q => q.IsSQL == questionId).FirstOrDefaultAsync();
            if (question != null)
            {
                question.D_Date = DateTime.Now;
                question.D_Id = userId;
                await _dataContext.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<IQueryable<SQL_Question>> GetSQL_Questions(int currentPage)
        {
            if (currentPage < 0)
            {
                return null;
            }
            IQueryable<SQL_Question> response;
            Pagination page = new Pagination();
            page.currentPage = currentPage;

            // get all the SQL question that are registered in the Question bank
            IQueryable<SQL_Question> sqlQuestions = (from sql in _dataContext.SQL_Question
                                                     join qu in _dataContext.Questions
                                                     on sql.QuestionId equals qu.IsSQL
                                                     where (qu.D_Date == null)
                                                     select sql);

            page.pageCount = (int)Math.Ceiling(sqlQuestions.Count() / page.pageSize);

            response = sqlQuestions.
                Skip((currentPage - 1) * (int)page.pageSize).
                Take((int)page.pageSize);
            return response;
        }

        public async Task<List<QuestionType>> GetQuestionTypes()
        {
            return await _dataContext.QuestionType.ToListAsync();
        }

        public async Task<string> GetQuestionType(int questionTypeId)
        {
            // get the question
            QuestionType? question = await _dataContext.QuestionType.FindAsync(questionTypeId);
            if (question == null)
            {
                return null;
            }
            return question.Type;
        }

        public async Task<string> EditQuestion(int id,Question updated)
        {
            var question = await _dataContext.Questions.FindAsync(id);
            if(question is null)
            {
                return null;
            }
            question.QuestionContent = updated.QuestionContent;
            question.Options = updated.Options;
            question.QuestionTypeId = updated.QuestionTypeId;
            question.TestTypeId = updated.TestTypeId;
            question.QuestionImage = updated.QuestionImage;
            question.U_Date = updated.U_Date;
            question.U_Id = updated.U_Id;

            await _dataContext.SaveChangesAsync();
            return "success";
        }

        public async Task<List<TestType>> GetAllTestTypes()
        {
            return await _dataContext.TestType.ToListAsync();
        }

        public async Task<List<Question>> Get_TestType_Questions(int testType)
        {
            if(testType == 0)
            {
                return new List<Question>();
            }
            IQueryable<Question> questions = _dataContext.Questions.Where(q => q.TestTypeId == testType && q.D_Date==null);
            return await questions.ToListAsync();
        }

        public async Task<List<Question>> Get_QuestionType_Questions(int questionType)
        {
            IQueryable<Question> questions = _dataContext.Questions.Where(q => q.QuestionTypeId == questionType && q.D_Date==null);
            return await questions.ToListAsync();
        }

        public static implicit operator QuestionService(Question v)
        {
            throw new NotImplementedException();
        }
    }
}
