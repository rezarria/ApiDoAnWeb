using Api.Areas.Edu.Contexts;
using Api.Areas.Edu.Models;
using Microsoft.AspNetCore.Mvc;
using RezUtility.Utilities;
using System.ComponentModel.DataAnnotations;

namespace Api.Areas.HeThong.Controllers;

[Area("HeThong")]
[Route("[area]/[controller]")]
public class DangKy : ControllerBase
{
	private readonly AppDbContext _appDbContext;


	public DangKy(AppDbContext appDbContext)
	{
		_appDbContext = appDbContext;
	}
	[HttpPost]
	public IActionResult Json([FromBody] MauDangKy mauDangKy)
	{
		if (ModelState.IsValid)
		{
			NguoiDung nguoiDung = new()
								  {
									  SoYeuLyLich = new()
												    {
													    Email = mauDangKy.Email,
													    HoVaTen = mauDangKy.HoVaTen,
													    NgaySinh = mauDangKy.NgaySinh,
												    },
									  TaiKhoan = new()
											     {
												     MatKhau = MatKhau.MaHoaMatKhau(mauDangKy.MatKhau)
											     }
								  };
			nguoiDung.TaiKhoan.Username = !string.IsNullOrEmpty(mauDangKy.Username) ? mauDangKy.Username : mauDangKy.Email;

			_appDbContext.NguoiDung.Attach(nguoiDung);
			_appDbContext.SaveChanges();

			return Ok();
		}
		return BadRequest(ModelState);
	}

	public class MauDangKy
	{
		[Required(AllowEmptyStrings = false)]
		public string Email { get; set; } = string.Empty;
		[Required(AllowEmptyStrings = false)]
		public string HoVaTen { get; set; } = String.Empty;
		public DateTime NgaySinh { get; set; }
		public string? Username { get; set; }
		[Required(AllowEmptyStrings = false)]
		public string MatKhau { get; set; } = String.Empty;
		public string? Avatar { get; set; }
	}
}