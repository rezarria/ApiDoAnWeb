using Api.Areas.Edu.Contexts;
using Api.Contexts;
using Api.Models.ElFinder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Areas.Admin.Controllers;

[Area("Admin")]
[Route("[area]/[controller]")]
public class ElFinderController : ControllerBase
{
	private readonly ElFinderDbContext _elFinderDbContext;
	private readonly AppDbContext _appDbContext;

	public ElFinderController(ElFinderDbContext elFinderDbContext, AppDbContext appDbContext)
	{
		_elFinderDbContext = elFinderDbContext;
		_appDbContext = appDbContext;
	}

	[HttpGet("DongBoTaiKhoan")]
	public async Task<IActionResult> DongBoTaiKhoan()
	{
		List<Guid> query1 = await _elFinderDbContext.User.Select(x => x.Id).ToListAsync(HttpContext.RequestAborted);
		List<Models.ElFinder.User> userMoi = await _appDbContext.TaiKhoan
															    .Where(x => !query1.Contains(x.Id))
															    .Select(x => new Models.ElFinder.User()
																	         {
																		         Id = x.Id,
																		         VolumePath = x.Id + x.Username
																	         })
															    .ToListAsync(HttpContext.RequestAborted);
		await _elFinderDbContext.User.AddRangeAsync(userMoi, HttpContext.RequestAborted);
		await _elFinderDbContext.SaveChangesAsync(HttpContext.RequestAborted);
		return Ok();
	}
}