using Api.Contexts;
using Microsoft.AspNetCore.Mvc;

namespace Api.Areas.Api.Controllers;

/// <summary>
/// </summary>
[Area("api")]
[ApiController]
[Route("/[area]/[controller]")]
public class SoYeuLyLich : ControllerBase
{
    private readonly AppDbContext _database;

    /// <summary>
    /// </summary>
    /// <param name="database"></param>
    public SoYeuLyLich(AppDbContext database)
    {
        _database = database;
    }

    /// <summary>
    /// </summary>
    /// <param name="soYeuLyLich"></param>
    /// <returns></returns>
    [HttpPost]
    public IActionResult Post([FromBody] Models.SoYeuLyLich.SoYeuLyLich soYeuLyLich)
    {
        _database.Add(soYeuLyLich);

        _database.SaveChanges();

        return Ok(soYeuLyLich);
    }
}