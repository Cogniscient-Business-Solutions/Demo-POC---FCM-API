using FirebaseNotificationAPI.Models.DTO;
using FirebaseNotificationAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FirebaseNotificationAPI.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FirebaseNotificationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly NotificationService _notificationService;

        public NotificationController(ITokenService tokenService, NotificationService notificationService)
        {
            _tokenService = tokenService;
            _notificationService = notificationService;
        }

        // Login API: Registers a user's FCM token
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.UserId) || string.IsNullOrEmpty(request.Token))
            {
                return BadRequest("UserId and Token are required.");
            }

            await _tokenService.SaveTokenAsync(request.UserId, request.Token);
            return Ok("Login successful and token saved.");
        }

        // Logout API: Removes a user's FCM token
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
        {
            if (string.IsNullOrEmpty(request.UserId))
            {
                return BadRequest("UserId is required.");
            }

            await _tokenService.DeleteTokenAsync(request.UserId);
            return Ok("Logout successful and token deleted.");
        }

        // Notification API: Sends a notification to multiple users
        [HttpPost("send-notification")]
        public async Task<IActionResult> SendNotification([FromBody] NotificationRequest request)
        {
            if (request.UserIds == null || request.UserIds.Count == 0)
            {
                return BadRequest("UserIds are required.");
            }

            var tokens = await _tokenService.GetTokensByUserIdsAsync(request.UserIds);
            if (tokens.Count == 0)
            {
                return NotFound("No valid tokens found for the provided UserIds.");
            }

            var response = await _notificationService.SendNotificationAsync(tokens, request.Title, request.Body);
            return Ok(response);
        }
    }
}
