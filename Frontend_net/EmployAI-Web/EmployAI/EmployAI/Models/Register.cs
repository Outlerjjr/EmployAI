using System.ComponentModel.DataAnnotations;  // Imports the DataAnnotations namespace for validation attributes

namespace EmployAI.Models  // for grouping related code inside the models folder (to keep things organized)
{
    public class Register  // Declares the Register class to hold user registration data
    {
        public int Id { get; set; }  // Property for storing the user's unique ID

        public string Name { get; set; }  // Property for storing the user's full name

        // Validation attributes for the Email property
        [Required]  // Ensures that the Email field is required
        [EmailAddress]  // Validates that the input is in a valid email format
        public string Email { get; set; }  // Property for storing the user's email address

        public string Username { get; set; }  // Property for storing the user's username

        // Validation attributes for the Password property
        [Required]  // Ensures that the Password field is required
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long.")]  // Validates that the password is between 8 and 100 characters
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
        ErrorMessage = "Password must be at least 8 characters long and contain at least one special character, one number, one uppercase letter, and one lowercase letter.")]  // Validates the password against a pattern (at least 1 lowercase, 1 uppercase, 1 digit, and 1 special character)
        public string Password { get; set; }  // Property for storing the user's password
    }
}
