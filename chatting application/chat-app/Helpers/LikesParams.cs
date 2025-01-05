namespace chat_app.Helpers
{
    public class LikesParams: PaginationParams
    {
        public int UserId { get; set; }
        public required string Predicate { get; set; } = "liked";
    }
}
