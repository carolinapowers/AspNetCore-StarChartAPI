using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("api")]
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
            celestialObject.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObject.Id == celestialObject.Id).ToList();

            return Ok(celestialObject);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestialObjects = _context.CelestialObjects.Where(e => e.Name == name);
            foreach(var item in celestialObjects)
            {
                item.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObject.Id == item.Id).ToList();
            }

            return Ok(_context.CelestialObjects.Where(e => e.Name == name));
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var celestialObjects = _context.CelestialObjects;
            foreach (var item in celestialObjects)
            {
                item.Satellites = _context.CelestialObjects.Where(e => e.OrbitedObject.Id == item.Id).ToList();
            }

            return Ok(celestialObjects);
        }

        [HttpPost]
        public IActionResult Create([FromBody]CelestialObject celestialObject)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            _context.CelestialObjects.Add(celestialObject);
            _context.SaveChanges();
            return CreatedAtRoute("GetItem", new { id = celestialObject.Id }, celestialObject);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject celestialObject)
        {
            var item = _context.CelestialObjects.Find(id);
            if(item == null)
            {
                return NotFound();
            }

            item = celestialObject;

            _context.CelestialObjects.Update(celestialObject);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id}")]
        public IActionResult RenameObject(int id, string name)
        {
            var celestialObject = _context.CelestialObjects.Find(id);
            if(celestialObject == null)
            {
                return NotFound();
            }

            celestialObject.Name = name;

            _context.CelestialObjects.Update(celestialObject);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var celestialObjects = _context.CelestialObjects.Where(e => e.Id == id || e.OrbitedObject.Id == id).ToList();
            if (celestialObjects.Count == 0)
            {
                return NotFound();
            }

            _context.CelestialObjects.RemoveRange(celestialObjects);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
