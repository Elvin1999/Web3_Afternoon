using System.ComponentModel.DataAnnotations;

namespace Web3_Afternoon.Dtos
{
    public class CarDto
    {
        public int Id { get; set; }
        [Required]
        public string? Model { get; set; }
        [Required]
        public string? Vendor { get; set; }
    }
}
