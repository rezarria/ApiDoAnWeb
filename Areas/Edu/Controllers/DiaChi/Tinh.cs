using Api.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Areas.Api.Controllers;

/// <summary>
/// </summary>
[Area("Api")]
[Route("/[area]/[controller]")]
[ApiController]
public class Tinh : ControllerBase
{
    private readonly AppDbContext _database;

    /// <summary>
    /// </summary>
    /// <param name="database"></param>
    public Tinh(AppDbContext database)
    {
        _database = database;
    }

    /// <summary>
    /// </summary>
    /// <param name="tinh"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] Models.DiaChi.Tinh tinh)
    {
        Console.WriteLine("!");
        _database.Add(tinh);
        await _database.SaveChangesAsync(HttpContext.RequestAborted);
        return Ok(tinh);
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public Task<IActionResult> Get()
    {
        var list = _database.Tinh.Include(x => x.QuanHuyen).AsNoTracking().Select(x => new
        {
            x.Id,
            x.Ten,
            QuanHuyen = x.QuanHuyen!.Select(y => new
            {
                y.Id,
                y.Ten
            }).ToArray()
        }).ToArray();
        return System.Threading.Tasks.Task.FromResult<IActionResult>(Ok(list));
    }

    [HttpGet("time")]
    public async Task<IActionResult> GetTinhTime()
    {
        var ketQua = await _database.CaiDat.Where(x => x.Key == "ThongTinTinhHuyenVietNam__Update_Time").Select(x => x.Value).FirstOrDefaultAsync(HttpContext.RequestAborted);
        if (ketQua is not null)
            return Ok(ketQua);
        else return NotFound();
    }
}