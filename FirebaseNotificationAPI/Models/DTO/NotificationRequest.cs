namespace FirebaseNotificationAPI.Models.DTO
{
    public class NotificationRequest
    {
        public List<string> UserIds { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}
