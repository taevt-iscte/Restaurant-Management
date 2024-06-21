using System.ComponentModel.DataAnnotations;

namespace Restaurant_Management.Models
{
    public class Restaurant(string name, string description, string contact)
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = name;
        public string? Description { get; set; } = description;
        [Required]
        public string Contact { get; set; } = contact;
        [DataType(DataType.Date)]
        public DateTime CreatedDate { get; set; }
        public ICollection<Employee> Employees { get; set; }
    }
}