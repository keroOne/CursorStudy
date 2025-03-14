using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCInventoryManagement.API.Data;
using PCInventoryManagement.API.Models;

namespace PCInventoryManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PCLocationHistoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PCLocationHistoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/PCLocationHistories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PCLocationHistory>>> GetPCLocationHistories()
        {
            return await _context.PCLocationHistories
                .Include(h => h.PC)
                .Include(h => h.FromUser)
                .Include(h => h.ToUser)
                .OrderByDescending(h => h.CreatedAt)
                .ToListAsync();
        }

        // GET: api/PCLocationHistories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PCLocationHistory>> GetPCLocationHistory(int id)
        {
            var history = await _context.PCLocationHistories
                .Include(h => h.PC)
                .Include(h => h.FromUser)
                .Include(h => h.ToUser)
                .FirstOrDefaultAsync(h => h.Id == id);

            if (history == null)
            {
                return NotFound();
            }

            return history;
        }

        // GET: api/PCLocationHistories/PC/5
        [HttpGet("PC/{pcId}")]
        public async Task<ActionResult<IEnumerable<PCLocationHistory>>> GetPCLocationHistoriesByPC(int pcId)
        {
            return await _context.PCLocationHistories
                .Include(h => h.PC)
                .Include(h => h.FromUser)
                .Include(h => h.ToUser)
                .Where(h => h.PCId == pcId)
                .OrderByDescending(h => h.CreatedAt)
                .ToListAsync();
        }

        // POST: api/PCLocationHistories
        [HttpPost]
        public async Task<ActionResult<PCLocationHistory>> CreatePCLocationHistory(PCLocationHistory history)
        {
            history.CreatedAt = DateTime.UtcNow;
            history.UpdatedAt = DateTime.UtcNow;
            _context.PCLocationHistories.Add(history);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPCLocationHistory), new { id = history.Id }, history);
        }

        // PUT: api/PCLocationHistories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePCLocationHistory(int id, PCLocationHistory history)
        {
            if (id != history.Id)
            {
                return BadRequest();
            }

            var existingHistory = await _context.PCLocationHistories.FindAsync(id);
            if (existingHistory == null)
            {
                return NotFound();
            }

            existingHistory.Status = history.Status;
            existingHistory.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PCLocationHistoryExists(id))
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

        private bool PCLocationHistoryExists(int id)
        {
            return _context.PCLocationHistories.Any(e => e.Id == id);
        }
    }
} 