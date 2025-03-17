using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCInventoryManagement.API.Data;
using PCInventoryManagement.API.Models;

namespace PCInventoryManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PCsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PCsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetPCs()
        {
            var pcs = await _context.PCs
                .Include(p => p.OSType)
                .Include(p => p.User)
                .Where(p => !p.IsDeleted)
                .ToListAsync();

            return Ok(pcs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PC>> GetPC(int id)
        {
            var pc = await _context.PCs
                .Include(p => p.OSType)
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted);

            if (pc == null)
            {
                return NotFound();
            }

            return pc;
        }

        [HttpPost]
        public async Task<ActionResult<PC>> CreatePC(PC pc)
        {
            if (!await _context.OSTypes.AnyAsync(o => o.Id == pc.OSTypeId))
            {
                return BadRequest("Invalid OSTypeId");
            }

            if (pc.UserId.HasValue && !await _context.Users.AnyAsync(u => u.Id == pc.UserId))
            {
                return BadRequest("Invalid UserId");
            }

            if (await _context.PCs.AnyAsync(p => p.ManagementNumber == pc.ManagementNumber && !p.IsDeleted))
            {
                return BadRequest("Management number already exists");
            }

            pc.CreatedAt = DateTime.UtcNow;
            _context.PCs.Add(pc);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPC), new { id = pc.Id }, pc);
        }

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

            if (!await _context.OSTypes.AnyAsync(o => o.Id == pc.OSTypeId))
            {
                return BadRequest("Invalid OSTypeId");
            }

            if (pc.UserId.HasValue && !await _context.Users.AnyAsync(u => u.Id == pc.UserId))
            {
                return BadRequest("Invalid UserId");
            }

            if (await _context.PCs.AnyAsync(p => p.ManagementNumber == pc.ManagementNumber && p.Id != id && !p.IsDeleted))
            {
                return BadRequest("Management number already exists");
            }

            existingPC.ManagementNumber = pc.ManagementNumber;
            existingPC.OSTypeId = pc.OSTypeId;
            existingPC.UserId = pc.UserId;
            existingPC.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.PCs.AnyAsync(p => p.Id == id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePC(int id)
        {
            var pc = await _context.PCs.FindAsync(id);
            if (pc == null)
            {
                return NotFound();
            }

            pc.IsDeleted = true;
            pc.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
} 