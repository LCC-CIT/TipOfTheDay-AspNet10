namespace TipOfTheDay.Models
{
    public class Tip
    {
        private readonly List<Language> _languages = new();
        private readonly List<Tag> _tags = new();
        private readonly List<Comment> _comments = new();
        private string _tipText = "Default tip";

        public int TipId { get; set; }

        // Reference types are now non-nullable by default
        // Member is being made nullable to facilitate the scaffolded Edit feature
        // If non-nullable we get a ModelValidation error if it's not included in a HTTP Post to Edit.
        public AppUser? Member { get; set; }

        public List<Language> Languages { get => _languages; }

        public List<Tag> Tags{ get => _tags; }

        public List<Comment> Comments { get => _comments; }

        public String TipText { get => _tipText; set => _tipText = value; }

    }
}
