using System.ComponentModel.DataAnnotations;
using static ParksAPI.Models.Trail;

namespace ParksAPI.Models.DTOs
{
    public class TrailCreateDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public double Distance { get; set; }

        public DifficultyType Difficulty { get; set; }

        [Required]
        public int NationalParkId { get; set; }
    }
}
