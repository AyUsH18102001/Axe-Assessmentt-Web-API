using AxeAssessmentToolWebAPI.FrontendModel;
using AxeAssessmentToolWebAPI.Models;
using AxeAssessmentToolWebAPI.Response_Models;
using AxeAssessmentToolWebAPI.Services;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace AxeAssessmentToolWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]"), Authorize(Roles = "Admin")]
    public class QuestionController : ControllerBase
    {

        private readonly IQuestionService _service;

        public QuestionController(IQuestionService service)
        {
            this._service = service;
        }

        [HttpGet("{page}")]
        public async Task<IQueryable<Question>> GetQuestions(int page)
        {
            return await _service.GetQuestions(page);
        }

        [HttpGet("pageCount")]
        public async Task<int> GetQuestionPageCount()
        {
            return await _service.GetQuestionPageCount();
        }

        [HttpGet("questionTypes")]
        public async Task<List<QuestionType>> GetQuestionTypes()
        {
            return await _service.GetQuestionTypes();
        }

        [HttpGet("getQuestion/{id}")]
        public async Task<Question> GetQuestionByID(int id)
        {
            return await _service.GetQuestionByID(id);
        }

        [HttpGet("getSqlQuestion/{id}")]
        public async Task<SQL_TestData> GetSqlQuestionById(int id)
        {
            return await _service.GetSqlQuestionById(id);
        }

        [HttpPost("addQuestion")]
        public async Task<MessageAndCode> AddQuestion(Question question)
        {
            MessageAndCode response = new MessageAndCode();
            var result = await _service.AddQuestion(question);
            if (result != "success")
            {
                response.Message = result;
                response.error = true;
                return response;
            }
            response.Message = result;
            return response;
        }

        [HttpPost("addQuestionImage")]
        public async Task<MessageAndCode> AddQuestionImage()
        {
            var img = Request.Form.Files[0];
            MessageAndCode response = new MessageAndCode();
            var result = await _service.AddQuestionImage(img);
            response.Message = result;
            return response;
        }

        [HttpGet("getQuestionImage/{questionId}")]
        public async Task<MessageAndCode> GetQuestionImage(int questionId)
        {
            MessageAndCode response = new MessageAndCode();
            var result = await this._service.GetQuestionImage(questionId);
            response.Message = result;
            return response;
        }

        [HttpGet("lastQuestion")]
        public async Task<Question> LastQuestion()
        {
            return await this._service.LastQuestion();
        }

        [HttpPatch("deleteQuestion")]
        public async Task<MessageAndCode> DeleteQuestion(PatchQuestion patch)
        {
            MessageAndCode response = new MessageAndCode();
            var result = await _service.DeleteQuestion(patch);
            if (result is null)
            {
                response.Message = "No Question of this id is found";
            }
            else
            {
                response.Message = result;
            }
            return response;
        }

        [HttpGet("getOptions/{questionId}")]
        public async Task<List<Option>> GetQuestionOptions(int questionId)
        {
            return await this._service.GetQuestionOptions(questionId);
        }

        [HttpPost("addBulkQuestions")]
        public async Task<MessageAndCode> AddBulkQuestions(List<Question> questions)
        {
            MessageAndCode response = new MessageAndCode();
            response.Message = await _service.AddBulkQuestions(questions);
            if (response.Message != "success")
            {
                response.error = true;
                return response;
            }
            return response;
        }

        [HttpPut("updateQuestion/{id}")]
        public async Task<MessageAndCode> EditQuestion(int id, Question updated)
        {
            MessageAndCode response = new MessageAndCode();
            var result = await _service.EditQuestion(id, updated);
            if (result is null)
            {
                response.Message = "No Question of this id is found";
            }
            response.Message = result;
            return response;
        }

        [HttpGet("getSqlQuestions/{currentPage}")]
        public async Task<IQueryable<SQL_Question>> GetSQL_Questions(int currentPage)
        {
            return await _service.GetSQL_Questions(currentPage);
        }

        [HttpGet("getSqlQuestionPageCount")]
        public async Task<int> GetSqlQuestionsPageCount()
        {
            return await _service.GetSqlQuestionsPageCount();
        }

        [HttpPatch("deleteSqlQuestion")]
        public async Task<MessageAndCode> DeleteSqlQuestion(PatchQuestion patch)
        {
            MessageAndCode response = new MessageAndCode();
            bool result = await _service.DeleteSqlQuestion(patch.questionId, patch.adminId);
            if (result)
            {
                response.Message = "success";
                return response;
            }
            return response;
        }

        [HttpGet("getQuestionType/{questionTypeId}")]
        public async Task<MessageAndCode> GetQuestionType(int questionTypeId)
        {
            MessageAndCode response = new MessageAndCode();
            var result = await _service.GetQuestionType(questionTypeId);
            if (result is null)
            {
                response.Message = "";
            }
            response.Message = result;
            return response;
        }

        [HttpGet("getTestTypes")]
        public async Task<List<TestType>> GetAllTestTypes()
        {
            return await _service.GetAllTestTypes();
        }

        [HttpGet("getTestType_questions/{testType}")]
        public async Task<List<Question>> Get_TestType_Questions(int testType)
        {
            return await _service.Get_TestType_Questions(testType);
        }

        [HttpGet("getQuestionType_questions/{questionType}")]
        public async Task<List<Question>> Get_QuestionType_Questions(int questionType)
        {
            return await _service.Get_QuestionType_Questions(questionType);
        }
    }
}
