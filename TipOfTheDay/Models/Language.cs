namespace TipOfTheDay.Models
{
    public class Language
    {
        private string _language = "Default language";
        public int LanguageId { get; set; }
        public String Name { get => _language; set => _language = value; }
    }
}
