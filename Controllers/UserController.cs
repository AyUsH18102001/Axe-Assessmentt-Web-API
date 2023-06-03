using AxeAssessmentToolWebAPI.FrontendModel;
using AxeAssessmentToolWebAPI.Models;
using AxeAssessmentToolWebAPI.Response_Models;
using AxeAssessmentToolWebAPI.Services;
using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AxeAssessmentToolWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {

        private readonly IUserService _service;
        public UserController(IUserService service)
        {
            this._service = service;
        }

        [HttpGet("getUsers/{page}")]
        public async Task<IQueryable<User>> GetAllUser(int page)
        {
            return await _service.GetAllUser(page);
        }

        [HttpGet("pageCount")]
        public async Task<int> GetUserPageCount()
        {
            return await this._service.GetUserPageCount();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var result = await _service.GetUser(id);
            if (result == null)
            {
                return NotFound("");
                // Create the constants file
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<MessageAndCode> RegisterUser(User user)
        {
            MessageAndCode result = await _service.RegisterUser(user);
            return result;
        }

        [HttpPost("login")]
        public async Task<Token> LoginUser(Login creds)
        {
            Token result = await _service.LoginUser(creds);
            if (result == null)
            {
                return null;
            }
            return result;
        }

        [HttpPost("testSubmit")]
        public async Task<MessageAndCode> UserSubmitTest(TestSubmit answers)
        {
            var result = await _service.UserSubmitTest(answers);
            MessageAndCode response = new MessageAndCode();
            response.Message = result;
            return response;
        }

        [HttpPost("questionAttempted")]
        public async Task<MessageAndCode> UpdateQuestionAttempted(QuestionAttempted qa)
        {
            var result = await _service.UpdateQuestionAttempted(qa.userId,qa.questionId,qa.optionsIndex);
            MessageAndCode response = new MessageAndCode();
            response.Message = result;
            return response;
        }

        /*[HttpPost("optionSelected")]
        public async Task<MessageAndCode> UpdateOptionSelected(OptionSelected os)
        {
            var result = await _service.UpdateOptionSelected(os.questionId, os.optionsIndex);
            MessageAndCode response = new MessageAndCode();
            response.Message = result;
            return response;
        }*/

        [HttpPost("loginToken")]
        public async Task<Token> LoginWithUserToken(LoginToken userToken)
        {
            string result = await _service.LoginWithUserToken(userToken.Token);
            Token token = new Token();
            if (!string.IsNullOrEmpty(result))
            {
                token.token = result;
                return token;
            }
            token.error = "Invalid Token";
            return token;
        }

        [HttpPost("uploadResume")]
        public async Task<MessageAndCode> UploadResume()
        {
            var resume = Request.Form.Files[0];
            MessageAndCode response = new MessageAndCode();

            //save file
            string result = await _service.UploadResume(resume);
            if (!string.IsNullOrEmpty(result))
            {
                response.Message = "Resume Uploaded successfully";
            }
            else
            {
                response.Message = "Error Ocurred in uploading Resume";
            }
            return response;
        }

        [HttpGet("userResume/{userId}")]
        public async Task<MessageAndCode> GetUserResume(int userId)
        {
            MessageAndCode response = new MessageAndCode();
            response.Message = await this._service.GetUserResume(userId);
            return response;
        }

        [HttpPost("uploadProfile")]
        public async Task<MessageAndCode> UploadUserProfile()
        {
            var image = Request.Form.Files[0];
            MessageAndCode response = new MessageAndCode();
            //save file
            string result = await _service.UploadUserProfile(image);
            if (!string.IsNullOrEmpty(result))
            {
                response.Message= "Profile Uploaded successfully";
            }
            else
            {
                response.Message = "Error Ocurred in uploading profle";
            }
            return response;
        }

        [HttpGet("userImage/{userId}")]
        public async Task<MessageAndCode> GetUserProfileImage(int userId)
        {
            MessageAndCode response = new MessageAndCode();
            response.Message =  await this._service.GetUserProfileImage(userId);
            return response;
        }

        [HttpGet("userTest")]
        public async Task<IActionResult> GetUserTest(string email)
        {
            var result = await _service.GetUserTest(email);
            if (result != null)
            {
                return Ok(result);
            }
            return NotFound("No user exists");
        }

        [HttpGet("testData/{userId}")]
        public async Task<List<TestData>> GetTestData(int userId)
        {
            return await _service.GetTestData(userId);
        }

        [HttpGet("getSqlTestData/{userId}")]
        public async Task<IEnumerable<SQL_TestData>> SqlTestData(int userId)
        {
            return await _service.SqlTestData(userId);
        }

        [HttpGet("getTestRules/{userId}")]
        public async Task<List<CandidateRules>> GetTestRules(int userId)
        {
            return await _service.GetTestRules(userId);
        }

        [HttpGet("userTerminated")]
        public async Task<IActionResult> IsUserTerminated(int userId)
        {
            bool result = await _service.IsUserTerminated(userId);
            if (!result)
            {
                return Ok("The User is Not Terminated");
            }
            return BadRequest("User Terminated");
        }

        [HttpGet("userViolation")]
        public async Task<IActionResult> GetUserViolation(int userId)
        {
            int result = await _service.GetUserViolation(userId);
            if(result == -1)
            {
                return NotFound("No user found");
            }
            return Ok(result.ToString());
        }

        [HttpPut("updateUserViolation/{userId}")]
        public async Task<IActionResult> UpdateUserViolation(int userId)
        {
            bool result = await _service.UpdateUserViolation(userId);
            if (result)
            {
                return Ok("Volation Updated");
            }
            return BadRequest("Error Ocurred in updating user violation");
        }


        [HttpGet("userEmailAndId"),Authorize]
        public async Task<UserData> GetUserEmailAndId()
        {
            return await _service.GetUserEmailAndId();
        }

        [HttpPost("runSql")]
        public async Task<SQL_Response> RunSqlQuery(UserQuery query)
        {
            return await _service.RunSqlQuery(query);
        }

        [HttpPost("forgotPassword")]
        public async Task<MessageAndCode> ForgotPassword(ForgotPassword userCreds)
        {
            string result = await _service.ForgotPassword(userCreds.Email, userCreds.Password);
            MessageAndCode response = new MessageAndCode();
            response.Message = result;
            return response;
        }

        [HttpPost("updateScore")]
        public async Task<IActionResult> UpdateScore(int userId, int questionId, List<int> options)
        {
            bool result = await _service.UpdateScore(userId, questionId, options);
            if (result)
            {
                return Ok("Score has been updated");
            }
            return BadRequest("Error Ocurred while updating the score");
        }

        [HttpPut("updateEndTest")]
        public async Task<MessageAndCode> UpdateEndTest(UserData user)
        {
            bool result = await _service.UpdateEndTest(user.UserId);
            MessageAndCode response = new MessageAndCode();
            if (result)
            {
                response.Message = "success";
                return response;
            }
            response.Message = "";
            return response;
        }

        [HttpPut("updateSqlScore")]
        public async Task<MessageAndCode> UpdateSqlScore(UserData user)
        {
            MessageAndCode response = new MessageAndCode();
            var result = await _service.UpdateSqlScore(user.UserId);
            if (result == null)
            {
                return response;
            }
            response.Message = result;
            return response;
        }

        [HttpPost("submitSqlQuery")]
        public async Task<MessageAndCode> SubmitQuery(List<SubmitSql> queries)
        {
            MessageAndCode response = new MessageAndCode();
            var result = await _service.SubmitQuery(queries);
            if (result == null)
            {
                return response;
            }
            response.Message = result;
            return response;
        }

        [HttpGet("getTestName/{userId}")]
        public async Task<MessageAndCode> UserTestName(int userId)
        {
            MessageAndCode response = new MessageAndCode();
            string result = await _service.UserTestName(userId);
            if (string.IsNullOrEmpty(result))
            {
                response.error = true;
                return response;
            }
            response.Message = result;
            return response;
        }

        [HttpGet("getTestQuestions/{userId}")]
        public async Task<CandidateTest_QuestionsData> GetTestQuestions(int userId)
        {
            
            return await _service.GetTestQuestions(userId);
        }

        [HttpGet("getTestInformation/{userId}")]
        public async Task<TestInfo> GetTestInformation(int userId)
        {
            return await _service.GetTestInformation(userId);
        }

    }
}
