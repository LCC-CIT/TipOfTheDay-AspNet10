namespace TipOfTheDay.Models
{
    public class TagChoice
    {
        public bool Selected { get; set; }  // false by default
        public Tag Tag { get; set; }
    }


    public class TipTagVM
    {
        private readonly List<TagChoice> _tagSelections = new();

        public Tip Tip { get; set; }
        public List<TagChoice> TagSelections { get => _tagSelections; }
    }
}
