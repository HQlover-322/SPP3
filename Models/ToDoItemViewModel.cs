using System.ComponentModel.DataAnnotations;

namespace back.Models
{
    public class ToDoItemViewModel
    {
        [Required]
        public string Task { get; set; }
        public bool IsDone { get; set; }
        [Required]
        public DateTime MyDateTime { get; set; }
        public List<IFormFile> MyRes { get; set; } = new List<IFormFile>();
        public List<string> FileNames {get; set; } = new List<string>();
        public Guid Id { get; set; } 
    }
}
