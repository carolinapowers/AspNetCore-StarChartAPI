using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var celestialObject = _context.CelestialObjects.Find(id);
            if (celestialObject == null)
                return NotFound();

            celestialObject.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObject.Id == celestialObject.Id).ToList();

            return Ok(celestialObject);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestialObjects = _context.CelestialObjects.Where(e => e.Name == name);
            if (celestialObjects == null)
                return NotFound();

            foreach(var item in celestialObjects)
            {
                item.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObject.Id == item.Id).ToList();
            }

            return Ok(_context.CelestialObjects.Where(e => e.Name == name));
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var celestialObjects = _context.CelestialObjects.ToList();
            foreach (var item in celestialObjects)
            {
                item.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObject.Id == item.Id).ToList();
            }

            return Ok(celestialObjects);
        }
    }
}
