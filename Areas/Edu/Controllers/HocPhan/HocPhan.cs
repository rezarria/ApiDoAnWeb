using Microsoft.AspNetCore.Mvc;

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




}
