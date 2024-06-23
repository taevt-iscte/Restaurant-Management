using Microsoft.AspNetCore.Mvc;
using Restaurant_Management.Models;
using AutoMapper;
using Restaurant_Management.Dto;
using Restaurant_Management.Interfaces;

namespace Restaurant_Management.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RestaurantController(ILogger<RestaurantController> logger, IRestaurantRepository context, IEmployeeRepository employeeRepository, IMapper mapper) : ControllerBase
    {
        private readonly IRestaurantRepository _restaurantRepository = context;
        private readonly ILogger<RestaurantController> _logger = logger;
        private readonly IEmployeeRepository _employeeRepository = employeeRepository;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<RestaurantDto>))]
        public IActionResult GetRestaurants()
        {
            return Ok(_mapper.Map<List<RestaurantDto>>(_restaurantRepository.GetRestaurants()));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RestaurantDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetRestaurant(int id)
        {
            RestaurantDto? rest = _mapper.Map<RestaurantDto>(_restaurantRepository.GetRestaurant(id));
            if (rest == null)
                return NotFound();
            if (!ModelState.IsValid)
                return BadRequest();
            return Ok(rest!);
        }

        [HttpGet("{id}/employees")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<EmployeeDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetRestaurantEmployees(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            Restaurant rest = _restaurantRepository.GetRestaurant(id);
            if (rest == null)
                return NotFound();
            return Ok(_mapper.Map<List<EmployeeDto>>(_employeeRepository.GetEmployeesByRestaurant(id)));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(int))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult CreateRestaurant([FromBody] RestaurantDto restDto)
        {
            if (restDto == null || !ModelState.IsValid)
                return BadRequest(ModelState);
            if (_restaurantRepository.GetRestaurants().Where(rest => rest.Name.Equals(restDto.Name, StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault() != null)
            {
                ModelState.AddModelError("Existent", "Restaurant already exists");
                return StatusCode(StatusCodes.Status422UnprocessableEntity, ModelState);
            }
            Restaurant rest = _mapper.Map<Restaurant>(restDto);
            rest.CreatedDate = DateTime.Now;
            int id = _restaurantRepository.CreateRestaurant(rest);
            if (id < 0)
            {
                ModelState.AddModelError("Internal", "Something went wrong while saving");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }
            return Ok(id);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult UpdateRestaurant(int id, [FromBody]RestaurantDto restaurantDto)
        {
            if (!ModelState.IsValid || restaurantDto == null || restaurantDto.Id != id) 
            {
                return BadRequest(ModelState);
            }
            if (!_restaurantRepository.GetRestaurants().Where(r => r.Id == id).Any())
            {
                return NotFound();
            }
            Restaurant rest = _mapper.Map<Restaurant>(restaurantDto);
            if (!_restaurantRepository.UpdateRestaurant(rest))
            {
                ModelState.AddModelError("Internal", "Something went wrong saving");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }
            return NoContent();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteRestaurant(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (_restaurantRepository.GetRestaurant(id) == null)
                return NotFound();
            if (!_restaurantRepository.RemoveRestaurant(id))
            {
                ModelState.AddModelError("Internal", "Something went wrong while deleting");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }
            return NoContent();
        }
    }
}
