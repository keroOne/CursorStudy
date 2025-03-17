using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PCInventoryManagement.API.Data;
using PCInventoryManagement.API.Models;

namespace PCInventoryManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LocationsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public LocationsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Location>>> GetLocations()
    {
        var locations = await _context.Locations
            .Where(l => !l.IsDeleted)
            .ToListAsync();
        return Ok(locations);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Location>> GetLocation(int id)
    {
        var location = await _context.Locations.FindAsync(id);

        if (location == null || location.IsDeleted)
        {
            return NotFound();
        }

        return location;
    }

    [HttpPost]
    public async Task<ActionResult<Location>> CreateLocation(Location location)
    {
        // 拠点コードの重複チェック
        if (await _context.Locations.AnyAsync(l => l.Code == location.Code && !l.IsDeleted))
        {
            return BadRequest("この拠点コードは既に使用されています。");
        }

        location.IsDeleted = false;
        _context.Locations.Add(location);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetLocation), new { id = location.Id }, location);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateLocation(int id, Location location)
    {
        if (id != location.Id)
        {
            return BadRequest();
        }

        // 拠点コードの重複チェック（自分以外）
        if (await _context.Locations.AnyAsync(l => l.Code == location.Code && l.Id != id && !l.IsDeleted))
        {
            return BadRequest("この拠点コードは既に使用されています。");
        }

        var existingLocation = await _context.Locations.FindAsync(id);
        if (existingLocation == null || existingLocation.IsDeleted)
        {
            return NotFound();
        }

        existingLocation.Code = location.Code;
        existingLocation.Name = location.Name;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!LocationExists(id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLocation(int id)
    {
        var location = await _context.Locations.FindAsync(id);
        if (location == null)
        {
            return NotFound();
        }

        location.IsDeleted = true;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool LocationExists(int id)
    {
        return _context.Locations.Any(e => e.Id == id);
    }
} 