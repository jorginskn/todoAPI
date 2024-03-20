using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using todoAPI.Model;

namespace todoAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly TodoContext _context;

        public ItemsController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/Items
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Item>>> GetTodoItems()
        {
          if (_context.TodoItems == null)
          {
              return NotFound();
          }
            

            if (!User.Identity?.IsAuthenticated ?? false)
            {
               return await _context.TodoItems.ToListAsync();
            }else
            {
                return  Forbid();
            } 
        }

        // GET: api/Items/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> GetItem(long id)
        {
          if (_context.TodoItems == null)
          {
              return NotFound();
          }
            var item = await _context.TodoItems.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            return item;
        }

        // PUT: api/Items/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutItem(long id, Item item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            _context.Entry(item).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Items
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Item>> PostItem(Item item)
        {
          if (_context.TodoItems == null)
          {
              return Problem("Entity set 'TodoContext.TodoItems'  is null.");
          }
            _context.TodoItems.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetItem", new { id = item.Id }, item);
        }

        // DELETE: api/Items/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(long id)
        {
            if (_context.TodoItems == null)
            {
                return NotFound();
            }
            var item = await _context.TodoItems.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ItemExists(long id)
        {
            return (_context.TodoItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
