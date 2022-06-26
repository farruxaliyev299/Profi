using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Profi.Models
{
    public class Testimonial
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Speciality { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int Rating { get; set; }
        public string Url { get; set; }

        [NotMapped,Required]
        public IFormFile Photo { get; set; }
    }
}
