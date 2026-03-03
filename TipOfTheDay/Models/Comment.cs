namespace TipOfTheDay.Models
{
    public class Comment
    {
        private string _commentText = "Default text";

        public int CommentId { get; set; }
        public AppUser Member { get; set; }
        public String CommentText { get => _commentText; set => _commentText = value; }
    }
}
