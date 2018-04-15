using EncodedComparer.Shared.Notifications;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace EncodedComparer.API.Controllers
{
    public class BaseController : Controller
    {
        public ActionResult ResponseBuilder(object data, bool success, string message, IEnumerable<Notification> notifications)
        {
            try
            {
                var responseObject = new { success, message, data, notifications };

                if (!success)
                    return BadRequest(responseObject);
                else
                    return Ok(responseObject);                
            }
            catch
            {
                return StatusCode(500, new { success = false, message = "An unexpected error occurred on server." });
            }
        }
    }
}
