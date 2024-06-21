namespace Restaurant_Management.Dto
{
    public class RestaurantDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string Contact { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
