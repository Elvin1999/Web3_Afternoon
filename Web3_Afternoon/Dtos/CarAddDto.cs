using System.ComponentModel.DataAnnotations;

namespace Web3_Afternoon.Dtos
{
    public class CarAddDto
    {
        [Required]
        public string? Model { get; set; }
        [Required]
        public string? Vendor { get; set; }
        public double Engine { get; set; }
        public int Year { get; set; }
    }
}
