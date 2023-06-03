using AxeAssessmentToolWebAPI.FrontendModel;
using AxeAssessmentToolWebAPI.Response_Models;
using AxeAssessmentToolWebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AxeAssessmentToolWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _service;
        public AdminController(IAdminService service)
        {
            this._service = service;
        }

        [HttpGet]
        public async Task<IActionResult> AddAdminProfile(int userId)
        {
            bool result = await _service.AddAdminProfile(userId);
            if (result)
            {
                return Ok("Admin profile added successfully");
            }
            return NotFound("No such user found");
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCandidateSelction(int userId, bool status)
        {
            bool result = await _service.UpdateCandidateSelction(userId,status);
            if (result)
            {
                return Ok("Selecton Status Updated");
            }
            return NotFound("Some error ocurred");
        }

        [HttpGet("getCandidateProfile/{userId}")]
        public async Task<MessageAndCode> GetCandidateProfile(int userId)
        {
            MessageAndCode response = new MessageAndCode();
            var result = await _service.GetCandidateProfile(userId);
            if (result == null)
            {
                response.error = true;
                return response;
            }
            response.Message = result;
            return response;
        }

        [HttpGet("downloadFormat")]
        public async Task<string> DownloadTestFormat()
        {
            return await _service.DownloadTestFormat();
        }

        [HttpGet("getAllTestRules")]
        public async Task<List<Rules>> GetAllTestRules()
        {
            return await _service.GetAllTestRules();
        }

        [HttpGet("getTestRules/{userId}")]
        public async Task<IQueryable<Rules>> GetTestRules(int userId)
        {
            return await _service.GetTestRules(userId);
        }

        [HttpGet("getTestRules_edit/{testId}")]
        public async Task<IQueryable<Rules>> GetTestRules_Edit(int testId)
        {
            return await _service.GetTestRules_Edit(testId);
        }

        [HttpPost("createTest")]
        public async Task<MessageAndCode> CreateTest(Test test)
        {
            MessageAndCode response = new MessageAndCode();
            var result = await _service.CreateTest(test);
            if (result == null)
            {
                response.Message = "Could not Create Test";
                return response;
            }
            response.Message = result;
            return response;
        }

        [HttpDelete("deleteTest/{deleteId}/{testId}")]
        public async Task<MessageAndCode> DeleteTest(int testId,int deleteId)
        {
            MessageAndCode response = new MessageAndCode();
            response.Message = await _service.DeleteTest(testId,deleteId);
            return response;
        }

        [HttpGet("getTestNames")]
        public async Task<IQueryable<TestDetails>> GetAllTest()
        {
            return await _service.GetAllTest();
        }

        [HttpGet("getTestPaper/{testId}")]
        public async Task<List<TestPaper>> GetTestPaperQuestions(int testId)
        {
            return await _service.GetTestPaperQuestions(testId);
        }

        [HttpPut("updateSelectionState/{userId}/{state}")]
        public async Task<MessageAndCode> UpateSelectionState(int userId, int state)
        {
            MessageAndCode response = new MessageAndCode();
            bool result = await _service.UpateSelectionState(userId, state);
            if (result)
            {
                response.Message = "success";
            }
            else
            {
                response.error = true;
            }
            return response;
        }

        [HttpPost("updateTest/{testId}")]
        public async Task<MessageAndCode> UpdateTest(int testId, Test updated)
        {
            MessageAndCode response = new MessageAndCode();
            string result = await _service.UpdateTest(testId, updated);
            if(result == "")
            {
                response.error = true;
                return response;
            }
            response.Message = result;
            return response;
        }

        [HttpPost("assignTest")]
        public async Task<MessageAndCode> UpdateUserTestId(AssignTest test)
        {
            MessageAndCode response = new MessageAndCode();
            var result = await _service.UpdateUserTestId(test.TestId,test.UserId);
            if (result != null)
            {
                response.Message = result;
                return response;
            }
            response.Message = "Error occured while assigning test";
            return response;
        }

        [HttpGet("getTestCreatorName/{creatorId}")]
        public async Task<MessageAndCode> GetTestCreator(int creatorId)
        {
            MessageAndCode response = new MessageAndCode();
            response.Message = await _service.GetTestCreator(creatorId);
            return response;
        }

        [HttpGet("getTableSchemas")]
        public async Task<List<TableDefinition>> GetTableSchemas()
        {
            return await _service.GetTableSchemas();
        }


    }
}
