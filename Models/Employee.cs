using System.ComponentModel.DataAnnotations;

namespace Restaurant_Management.Models
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? Address { get; set; }
        [Required]
        public Restaurant Restaurant { get; set; }
        [Required]
        public float GrossIncome { get; set; }
    }
}
