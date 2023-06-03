using AxeAssessmentToolWebAPI.Models;
using AxeAssessmentToolWebAPI.Response_Models;
using AxeAssessmentToolWebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace AxeAssessmentToolWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]"), Authorize(Roles = "Admin")]
    public class SqlQuestionController : Controller
    {

        private readonly ISqlQuestionService _service;

        public SqlQuestionController(ISqlQuestionService service)
        {
            this._service = service;
        }

        [HttpGet("getAllQuestion/{page}")]
        public async Task<IQueryable<SQL_Question>> GetAllSqlQuestions(int page)
        {
            return await _service.GetAllSqlQuestions(page);
        }


        [HttpGet("getPageCount")]
        public async Task<int> GetPageCount()
        {
            return await _service.GetPageCount();
        }

        [HttpPost]
        public async Task<MessageAndCode> AddQuestion(SQL_Question question)
        {
            MessageAndCode response = new MessageAndCode();
            var result = await _service.AddQuestion(question);
            response.Message = result;
            return response;
        }

        [HttpGet("{questionId}")]
        public async Task<SQL_Question> GetQuestion(int questionId)
        {
            SQL_Question result = await _service.GetQuestion(questionId);
            if (result == null)
            {
                return null;
            }
            return result;
        }

        [HttpDelete]
        public async Task<MessageAndCode> DeleteQuestion(int questionId)
        {
            MessageAndCode response = new MessageAndCode();
            var result = await _service.DeleteQuestion(questionId);
            response.Message = result;
            return response;
        }

        [HttpPost("updateSqlQuestion/{questionId}")]
        public async Task<MessageAndCode> UpdateQuestion(int questionId, SQL_Question updatedQuestion)
        {
            MessageAndCode response = new MessageAndCode();
            var result = await _service.UpdateQuestion(questionId,updatedQuestion);
            response.Message = result;
            return response;
        }

    }
}
