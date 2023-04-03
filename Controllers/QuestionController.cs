using AxeAssessmentToolWebAPI.Models;
using AxeAssessmentToolWebAPI.Services;
using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AxeAssessmentToolWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]"),Authorize(Roles ="Admin")]
    public class QuestionController : ControllerBase
    {

        private readonly IQuestionService _service;

        public QuestionController(IQuestionService service)
        {
            this._service = service;
        }

        [HttpGet]
        public async Task<List<Question>> GetQuestions()
        {
            return await _service.GetQuestions();
        }

        [HttpPost]
        public async Task<List<Question>> AddQuestion(Question question)
        {
            return await _service.AddQuestion(question);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteQuestion(int id)
        {
            var result = await _service.DeleteQuestion(id);
            if (result is null)
            {
                return NotFound("No SuperHero of this id is found");
            }
            return Ok(result);
        }

        [HttpPut("updateQuestion/{id}")]
        public async Task<IActionResult> EditQuestion(int id, Question updated)
        {
            var result = await _service.EditQuestion(id, updated);
            if (result is null)
            {
                return NotFound("No Question of this id is found");
            }
            return Ok(result);
        }
    }
}
