using Restaurant_Management.Models;

namespace Restaurant_Management.Interfaces
{
    public interface IRestaurantRepository
    {
        ICollection<Restaurant> GetRestaurants();
        int CreateRestaurant(Restaurant restaurant);
        bool RemoveRestaurant(int id);
        Restaurant GetRestaurant(int id);
        bool UpdateRestaurant(Restaurant restaurant);
        bool Save();
    }
}
