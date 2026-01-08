using Microsoft.AspNetCore.Mvc;

namespace NotificationService.Controllers
{
    [ApiController]
    [Route("api/notifications")]
    public class NotificationController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetNotification()
        {
            return Ok(new { Message = "Notification received" });
        }
    }
}
