using FirebaseAdmin.Messaging;

namespace FirebaseNotificationAPI.Services
{
    public class NotificationService
    {
        public async Task<string> SendNotificationAsync(List<string> tokens, string title, string body)
        {
            var message = new MulticastMessage()
            {
                Tokens = tokens,
                Notification = new Notification()
                {
                    Title = title,
                    Body = body
                }
            };

            var response = await FirebaseMessaging.DefaultInstance.SendMulticastAsync(message);
            return $"Successfully sent {response.SuccessCount} messages. Failed to send {response.FailureCount} messages.";
        }
    }
}
