using Api.Areas.Edu.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RezUtility.Utilities;

namespace Api.Areas.HeThong.Controllers;

[Area("HeThong")]
[Route("[area]/[controller]")]
[Authorize]
public class NguoiDungController : ControllerBase
{
	private AppDbContext _appDbContext;

	public NguoiDungController(AppDbContext appDbContext)
	{
		_appDbContext = appDbContext;
	}
	public IActionResult LayThongTin()
	{
		Guid idNguoiDung = Guid.Parse(User.Get("idNguoiDung"));
		Guid idTaiKhoan = Guid.Parse(User.Get("idTaikhoan"));

		return Ok();
	}
}