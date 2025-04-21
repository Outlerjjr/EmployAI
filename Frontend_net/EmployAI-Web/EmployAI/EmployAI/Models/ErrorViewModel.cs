namespace EmployAI.Models  // for grouping related code inside the models folder (to keep things organized)
{
    public class ErrorViewModel  // Defines a class named ErrorViewModel
    {
        public string? RequestId { get; set; }  // Property to store the RequestId, which is a unique identifier for an error request 

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);  // Property that checks if the RequestId is not null or empty, returning true if it exists, false otherwise
    }
}
