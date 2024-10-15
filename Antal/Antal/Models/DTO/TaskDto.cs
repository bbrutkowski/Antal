using System.ComponentModel.DataAnnotations;

namespace Antal.Models.DTO
{
    public record TaskDto
    {
        [Required(ErrorMessage = "Tytuł jest wymagany")]
        public string Title { get; set; }

        [StringLength(500, ErrorMessage = "Opis nie może przekraczać 500 znaków")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Data jest wymagana")]
        public DateTime Date {  get; set; }
    }
}
