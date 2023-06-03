using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using AxeAssessmentToolWebAPI.Services;
using AxeAssessmentToolWebAPI.Response_Models;

namespace AxeAssessmentToolWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]"), Authorize(Roles = "Admin")]
    public class EmailController : ControllerBase
    {

        private readonly IEmailService _service;

        public EmailController(IEmailService service)
        {
            this._service = service;
        }

        [HttpGet("{userId}/{testName}")]
        public async Task<MessageAndCode> SendEmail(int userId, string testName)
        {
            MessageAndCode response = new MessageAndCode();
            string result =  await _service.SendTokenEmail(userId,testName);
            if (!string.IsNullOrEmpty(result))
            {
                response.Message = "Email sent successfully";
            }
            else
            {
                response.Message = result;
            }
            return response;
        }
    }
}
