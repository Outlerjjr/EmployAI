namespace EmployAI.Models  // for grouping related code inside the models folder (to keep things organized)
{
    public class UplodeFile  // Declares the UplodeFile class to hold information about uploaded files
    {
        public string? Username { get; set; }  // Property to store the username of the person uploading the file (nullable)

        public string? Lang { get; set; }  // Property to store the language information related to the uploaded file (nullable)

        public IFormFile? Path { get; set; }  // Property to store the uploaded file
    }
}
