using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using AxeAssessmentToolWebAPI.Services;

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

        [HttpGet]
        public async Task<IActionResult> SendEmail(int userId)
        {
            string result =  await _service.SendTokenEmail(userId);
            if (string.IsNullOrEmpty(result))
            {
                return Ok("Email has been send Successfully");
            }
            return BadRequest(result);
        }
    }
}
