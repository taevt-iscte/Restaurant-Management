using Restaurant_Management.Data;
using Restaurant_Management.Interfaces;
using Restaurant_Management.Models;

namespace Restaurant_Management.Respository
{
    public class EmployeeRepository(ApplicationDBContext _context) : IEmployeeRepository
    {
        public int CreateEmployee(Employee employee)
        {
            _context.Add(employee);
            Save();
            return employee.Id;
        }

        public bool DeleteEmployee(Employee employee)
        {
            _context.Remove(employee);
            return Save();
        }

        public Employee? GetEmployee(int id)
        {
            return _context.Employees.Where(emp => emp.Id == id).FirstOrDefault();
        }

        public float? GetEmployeeGrossIncome(int id)
        {
            return _context.Employees.Where(emp => emp.Id == id).FirstOrDefault()?.GrossIncome;
        }

        public ICollection<Employee> GetEmployees()
        {
            return _context.Employees.ToList();
        }

        public ICollection<Employee> GetEmployeesByRestaurant(int restaurantId)
        {
            return _context.Employees.Where(e => e.Restaurant.Id == restaurantId).ToList();
        }

        public bool Save()
        {
            int saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool UpdateEmployee(Employee employee)
        {
            _context.ChangeTracker.Clear();
            _context.Update(employee);
            return Save();
        }
    }
}
