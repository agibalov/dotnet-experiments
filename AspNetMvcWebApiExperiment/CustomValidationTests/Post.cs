using System.ComponentModel.DataAnnotations;

namespace AspNetMvcWebApiExperiment.CustomValidationTests
{
    public class Post
    {
        [StringLength(10, ErrorMessage = "At most 10")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Not empty")]
        public string Text { get; set; }
    }
}