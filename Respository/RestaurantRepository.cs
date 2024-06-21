using Restaurant_Management.Data;
using Restaurant_Management.Interfaces;
using Restaurant_Management.Models;

namespace Restaurant_Management.Respository
{
    public class RestaurantRepository(ApplicationDBContext context) : IRestaurantRepository
    {
        private readonly ApplicationDBContext _context = context;

        public ICollection<Restaurant> GetRestaurants()
        {
            return _context.Restaurants.ToList();
        }

        public Restaurant? GetRestaurant(int id)
        {
            return _context.Restaurants.Where(rest => rest.Id == id).FirstOrDefault();
        }

        public int CreateRestaurant(Restaurant restaurant)
        {
            try
            {
                _context.Add(restaurant);
                Save();
                return restaurant.Id;
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public bool RemoveRestaurant(int id)
        {
            Restaurant rest = _context.Restaurants.Where(rest => rest.Id == id).First();
            foreach (Employee e in _context.Employees.Where(emp => emp.Restaurant.Id == id))
            {
                _context.Remove(e);
            }
            _context.Remove(rest);
            return Save();
        }

        public bool Save()
        {
            int saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool UpdateRestaurant(Restaurant restaurant)
        {
            _context.ChangeTracker.Clear();
            _context.Update(restaurant);
            return Save();
        }
    }
}
