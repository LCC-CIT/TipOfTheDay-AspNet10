namespace TipOfTheDay.Models
{
    public class Tag
    {
        private readonly List<Tip> _tips = new();
        private string _category = "Default category";

        public int TagId { get; set; }
        public String Category { get => _category; set => _category = value; }
        public List<Tip> Tips
        {
            get { return _tips; }
        }
    }
}
