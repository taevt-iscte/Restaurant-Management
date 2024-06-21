using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Restaurant_Management.Dto;
using Restaurant_Management.Interfaces;
using Restaurant_Management.Models;

namespace Restaurant_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController(IEmployeeRepository _employeeRepository, IRestaurantRepository _restaurantRepository, IMapper _mapper) : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Employee>))]
        public IActionResult GetEmployees()
        {
            return Ok(_mapper.Map<EmployeeDto>(_employeeRepository.GetEmployees()));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Employee))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetEmployee(int id)
        {
            if (!ModelState.IsValid || id <= 0)
                return BadRequest(ModelState);
            EmployeeDto employeeDto = _mapper.Map<EmployeeDto>(_employeeRepository.GetEmployee(id));
            if (employeeDto == null)
                return NotFound();
            return Ok(employeeDto);
        }

        [HttpGet("{id}/income")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(float))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetEmployeeGrossIncome(int id)
        {
            if (!ModelState.IsValid || id <= 0)
                return BadRequest(ModelState);
            float? income = _employeeRepository.GetEmployeeGrossIncome(id);
            if (income == null)
                return NotFound();
            return Ok(income);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult CreateEmployee([FromQuery] int restaurantId, [FromBody] EmployeeDto employeeDto)
        {
            if (!ModelState.IsValid || employeeDto == null)
                return BadRequest(ModelState);
            if (_employeeRepository.GetEmployees().Where(e => e.Name.Equals(employeeDto.Name, StringComparison.InvariantCultureIgnoreCase)).FirstOrDefault() != null)
            {
                ModelState.AddModelError("Existing", "Employee already exists");
                return BadRequest(ModelState);
            }
            Employee employee = _mapper.Map<Employee>(employeeDto);
            Restaurant restaurant = _restaurantRepository.GetRestaurant(restaurantId);
            if (restaurant == null)
                return NotFound();
            employee.Restaurant = restaurant;
            int id = _employeeRepository.CreateEmployee(employee);
            if (id < 0)
            {
                ModelState.AddModelError("Internal", "Something went wrong while saving");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }
            return Ok(id);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteEmployee(int id)
        {
            if (!ModelState.IsValid || id <= 0)
                return BadRequest(ModelState);
            Employee? emp = _employeeRepository.GetEmployee(id);
            if (emp == null)
                return NotFound();
            if (!_employeeRepository.DeleteEmployee(emp))
            {
                ModelState.AddModelError("Internal", "Something went wrong while saving");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }    
            return NoContent();
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateEmployee(int id, Employee emp)
        {
            if (emp == null || id <= 0 || !ModelState.IsValid)
                return BadRequest(ModelState);
            Employee? employee = _employeeRepository.GetEmployee(id);
            if (employee == null)
                return NotFound();
            if (!_employeeRepository.UpdateEmployee(employee))
            {
                ModelState.AddModelError("Internal", "Something went wrong while saving");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }
            return NoContent();
        }
    }
}
