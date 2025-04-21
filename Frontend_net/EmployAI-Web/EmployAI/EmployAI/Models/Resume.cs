namespace EmployAI.Models  // for grouping related code inside the models folder (to keep things organized)
{
    public class Resume  // Declares the Resume class to hold resume-related data
    {
        public string education { get; set; }  // Property to store the education details from the resume

        public string file_path { get; set; }  // Property to store the file path of the resume

        public string filename { get; set; }  // Property to store the filename of the resume

        public string job_title { get; set; }  // Property to store the job title listed in the resume

        public string resume_str { get; set; }  // Property to store the resume content as a string (text extracted from the resume)

        public int years_of_experience { get; set; }  // Property to store the number of years of experience mentioned in the resume
    }
}
