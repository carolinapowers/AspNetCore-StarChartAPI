using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WishList.Data;
using WishList.Models;

namespace WishList.Controllers
{
    [Route("/api/items")]
    public class ItemController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ItemController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id}", Name = "GetItem")]
        public IActionResult GetById(int id)
        {
            return Ok(_context.Items.Find(id));
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_context.Items.ToList());
        }

        [HttpPost]
        public IActionResult Create([FromBody]Item item)
        {
            _context.Items.Add(item);
            _context.SaveChanges();
            return CreatedAtRoute("GetItem", new { id = item.Id }, item);
        }

        [HttpPut]
        public IActionResult Update(int id, Item wishListItem)
        {
            var item = _context.Items.Find(id);
            if(item == null)
            {
                return NotFound();
            }

            item = wishListItem;

            _context.Items.Update(item);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPatch]
        public IActionResult UpdateDescription(int id, string description)
        {
            var item = _context.Items.Find(id);
            if(item == null)
            {
                return NotFound();
            }

            item.Description = description;

            _context.Items.Update(item);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var item = _context.Items.Find(id);
            if (item == null)
            {
                return NotFound();
            }

            _context.Items.Remove(item);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
