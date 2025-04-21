using System.ComponentModel.DataAnnotations;  // Imports the DataAnnotations namespace to use validation attributes

namespace EmployAI.Models  // for grouping related code inside the models folder (to keep things organized)
{
    public class FeedBack  // Declares the FeedBack class, which represents a user's feedback submission
    {
        public string? UserName { get; set; }  // Property to store the user's name (nullable type)

        // Validation attributes to ensure proper feedback details are provided
        [Required(ErrorMessage = "Feedback is required.")]  // Ensures that feedback is provided (cannot be empty)
        [MinLength(10, ErrorMessage = "Feedback must be at least 10 characters long.")]  // Ensures the feedback is at least 10 characters long
        public string DetailsFeedBack { get; set; }  // Property to store the actual feedback details
    }
}


