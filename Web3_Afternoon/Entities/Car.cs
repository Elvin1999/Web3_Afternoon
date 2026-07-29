using System.ComponentModel.DataAnnotations;

namespace Web3_Afternoon.Entities
{
    public class Car
    {
        public int Id { get; set; }
        [Required]
        public string? Model { get; set; }
        [Required]
        public string? Vendor { get; set; }
        public double Engine { get; set; }
        public int Year { get; set; }
    }
}
