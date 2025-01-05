namespace chat_app.Helpers
{
    public class MessageParams: PaginationParams
    {
        public string? Username { get; set; }
        public string Container { get; set; } = "Unread";
    }
}
