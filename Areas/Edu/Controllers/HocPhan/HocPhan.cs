using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Areas.Api.Controllers.HocPhan;

[ApiController]
[Area("Api")]
[Route("/[area]/[controller]")]
public class HocPhan : ControllerBase
{
    private readonly Contexts.AppDbContext _context;

    public HocPhan(Contexts.AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Get(string? ids)
    {
        var array = from x in ids?.Split(";") select new Guid(x);
        if (ids is null)
            return Ok(_context.HocPhan.AsNoTracking());
        return Ok(
            from x in _context.HocPhan
            where array.Contains(x.Id)
            select x
        );
    }
}
