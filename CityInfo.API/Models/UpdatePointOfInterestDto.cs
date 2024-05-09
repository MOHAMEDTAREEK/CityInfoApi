using System.ComponentModel.DataAnnotations;

namespace CityInfo.API.Models
{
    public class UpdatePointOfInterestDto
    {
        [Required(ErrorMessage = "You Should provide a Name Value")]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        [MaxLength(200)]
        public string? Description { get; set; }
    }
}
