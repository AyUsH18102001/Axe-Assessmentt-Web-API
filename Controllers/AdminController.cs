using AxeAssessmentToolWebAPI.Services;
using Microsoft.AspNetCore.Mvc;

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


    }
}
