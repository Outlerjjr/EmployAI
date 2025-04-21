namespace EmployAI.Models  // for grouping related code inside the models folder (to keep things organized)
{
    public class AnalyzeForm  // Declares the class AnalyzeForm, which will hold form data for analysis
    {
        public string? Username { get; set; }  // Property to store the username of the user submitting the form (nullable type)

        public List<string> Lang { get; set; }  // Property to store a list of languages associated with the form submission

        public List<IFormFile> Path { get; set; } = new List<IFormFile>();  // Property to store a list of uploaded files (defaulting to an empty list)
    }
}
