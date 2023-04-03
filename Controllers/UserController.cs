using AxeAssessmentToolWebAPI.Models;
using AxeAssessmentToolWebAPI.Services;
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

        [HttpGet]
        public async Task<List<User>> GetAllUser()
        {
            return await _service.GetAllUser();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var result = await _service.GetUser(id);
            if (result == null)
            {
                return NotFound("No such user found");
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> RegisterUser(User user)
        {
            var result = await _service.RegisterUser(user);
            if (!result)
            {
                return BadRequest("Error Ocurred in registering the user");
            }
            return Ok("New user has been registered");
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUser(string email, string password)
        {
            var result = await _service.LoginUser(email, password);
            if (!string.IsNullOrEmpty(result))
            {
                return Ok(result);
            }
            return NotFound("No such user have been registered");
        }

        [HttpPost("loginToken")]
        public async Task<IActionResult> LoginWithUserToken(string token)
        {
            string result = await _service.LoginWithUserToken(token);
            if (!string.IsNullOrEmpty(result))
            {
                return Ok(result);
            }
            return NotFound("No token found in database");
        }

        [HttpPost("uploadResume")]
        public async Task<IActionResult> UploadResume(int userId,IFormFile resume)
        {
            //save file
            string result = await _service.UploadResume(userId,resume);
            if (!string.IsNullOrEmpty(result))
            {
                return Ok("Resume Uploaded successfully");
            }
            return BadRequest("Error Ocurred in uploading Resume");
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

        [HttpGet("testData")]
        public async Task<List<TestData>> GetTestData()
        {
            return await _service.GetTestData();
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

        [HttpPut("updateUserViolation")]
        public async Task<IActionResult> UpdateUserViolation(int userId)
        {
            bool result = await _service.UpdateUserViolation(userId);
            if (result)
            {
                return Ok("Volation Updated");
            }
            return BadRequest("Error Ocurred in updating user violation");
        }

        [HttpGet("userId"),Authorize]
        public async Task<string> GetUserId()
        {
            return await _service.GetUserId();
        }
        
        [HttpGet("userEmail"),Authorize]
        public async Task<string> GetUserEmail()
        {
            return await _service.GetUserEmail();
        }

        [HttpPost("forgotPassword")]
        public async Task<IActionResult> ForgotPassword(string email, string newPassword)
        {
            bool result = await _service.ForgotPassword(email, newPassword);
            if (result)
            {
                return Ok("Password has been updated");
            }
            return BadRequest("Error Ocurred while updating the password");
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
        public async Task<IActionResult> UpdateEndTest(int userId)
        {
            bool result = await _service.UpdateEndTest(userId);
            if (result)
            {
                return Ok("End Test has been updated");
            }
            return BadRequest("Error Ocurred while updating end test");
        }

    }
}
