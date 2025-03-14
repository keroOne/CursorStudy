using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCInventoryManagement.API.Data;
using PCInventoryManagement.API.Models;

namespace PCInventoryManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OSTypesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public OSTypesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/OSTypes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<OSType>>> GetOSTypes()
        {
            return await _context.OSTypes
                .Where(o => !o.IsDeleted)
                .ToListAsync();
        }

        // GET: api/OSTypes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<OSType>> GetOSType(int id)
        {
            var osType = await _context.OSTypes
                .FirstOrDefaultAsync(o => o.Id == id && !o.IsDeleted);

            if (osType == null)
            {
                return NotFound();
            }

            return osType;
        }

        // POST: api/OSTypes
        [HttpPost]
        public async Task<ActionResult<OSType>> CreateOSType(OSType osType)
        {
            _context.OSTypes.Add(osType);
            await _context.SaveChangesAsync();

            var createdOSType = await _context.OSTypes
                .FirstOrDefaultAsync(o => o.Id == osType.Id);

            if (createdOSType == null)
            {
                return NotFound();
            }

            return createdOSType;
        }

        // PUT: api/OSTypes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOSType(int id, OSType osType)
        {
            if (id != osType.Id)
            {
                return BadRequest();
            }

            var existingOSType = await _context.OSTypes.FindAsync(id);
            if (existingOSType == null || existingOSType.IsDeleted)
            {
                return NotFound();
            }

            existingOSType.Name = osType.Name;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OSTypeExists(id))
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

        // DELETE: api/OSTypes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOSType(int id)
        {
            var osType = await _context.OSTypes.FindAsync(id);
            if (osType == null || osType.IsDeleted)
            {
                return NotFound();
            }

            osType.IsDeleted = true;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool OSTypeExists(int id)
        {
            return _context.OSTypes.Any(e => e.Id == id && !e.IsDeleted);
        }
    }
} 