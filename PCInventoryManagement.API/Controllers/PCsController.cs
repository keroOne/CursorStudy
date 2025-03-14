using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCInventoryManagement.API.Data;
using PCInventoryManagement.API.Models;

namespace PCInventoryManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PCsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PCsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/PCs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PC>>> GetPCs()
        {
            return await _context.PCs
                .Include(p => p.OSType)
                .Include(p => p.CurrentUser)
                .Where(p => !p.IsDeleted)
                .ToListAsync();
        }

        // GET: api/PCs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PC>> GetPC(int id)
        {
            var pc = await _context.PCs
                .Include(p => p.OSType)
                .Include(p => p.CurrentUser)
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            if (pc == null)
            {
                return NotFound();
            }

            return pc;
        }

        // POST: api/PCs
        [HttpPost]
        public async Task<ActionResult<PC>> CreatePC(PC pc)
        {
            pc.CreatedAt = DateTime.UtcNow;
            pc.UpdatedAt = DateTime.UtcNow;
            _context.PCs.Add(pc);
            await _context.SaveChangesAsync();

            var createdPC = await _context.PCs
                .Include(p => p.OSType)
                .Include(p => p.CurrentUser)
                .FirstOrDefaultAsync(p => p.Id == pc.Id);

            if (createdPC == null)
            {
                return NotFound();
            }

            return createdPC;
        }

        // PUT: api/PCs/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePC(int id, PC pc)
        {
            if (id != pc.Id)
            {
                return BadRequest();
            }

            var existingPC = await _context.PCs.FindAsync(id);
            if (existingPC == null || existingPC.IsDeleted)
            {
                return NotFound();
            }

            existingPC.ManagementNumber = pc.ManagementNumber;
            existingPC.ModelName = pc.ModelName;
            existingPC.OSTypeId = pc.OSTypeId;
            existingPC.CurrentUserId = pc.CurrentUserId;
            existingPC.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PCExists(id))
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

        // DELETE: api/PCs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePC(int id)
        {
            var pc = await _context.PCs.FindAsync(id);
            if (pc == null || pc.IsDeleted)
            {
                return NotFound();
            }

            pc.IsDeleted = true;
            pc.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PCExists(int id)
        {
            return _context.PCs.Any(e => e.Id == id && !e.IsDeleted);
        }
    }
} 