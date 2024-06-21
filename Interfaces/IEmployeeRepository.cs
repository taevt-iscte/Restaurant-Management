using Restaurant_Management.Models;

namespace Restaurant_Management.Interfaces
{
    public interface IEmployeeRepository
    {
        ICollection<Employee> GetEmployees();
        Employee? GetEmployee(int id);
        ICollection<Employee> GetEmployeesByRestaurant(int restaurantId);
        float? GetEmployeeGrossIncome(int id);
        int CreateEmployee(Employee employee);
        bool DeleteEmployee(Employee employee);
        bool UpdateEmployee(Employee employee);
        bool Save();
    }
}
