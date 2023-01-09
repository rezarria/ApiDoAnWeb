using System.ComponentModel.DataAnnotations;
using Api.Areas.Edu.Contexts;
using Api.Areas.Edu.Models;
using Api.Services;
using Microsoft.AspNetCore.Mvc;
using RezUtility.Utilities;

namespace Api.Areas.HeThong.Controllers;

[Area("HeThong")]
[Route("[area]/[controller]")]
public class DangKy : ControllerBase
{
	private readonly AppDbContext _appDbContext;
	private readonly QuanLyTepTin _quanLyTepTin;

	public DangKy(AppDbContext appDbContext, QuanLyTepTin quanLyTepTin)
	{
		_appDbContext = appDbContext;
		_quanLyTepTin = quanLyTepTin;
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

			if (!TryValidateModel(nguoiDung, nameof(nguoiDung)))
				return BadRequest(ModelState);

			_appDbContext.NguoiDung.Attach(nguoiDung);
			_appDbContext.SaveChanges();

			_quanLyTepTin.DongDoTaiKhoan().Wait();

			return Ok(nguoiDung.Id);
		}
		return BadRequest(ModelState);
	}

	public class MauDangKy
	{
		[Required(AllowEmptyStrings = false)]
		public string Email { get; set; } = string.Empty;
		[Required(AllowEmptyStrings = false)]
		public string HoVaTen { get; set; } = String.Empty;
		[DataType(DataType.Date)]
		public DateTime NgaySinh { get; set; }
		public string? Username { get; set; }
		[Required(AllowEmptyStrings = false)]
		public string MatKhau { get; set; } = String.Empty;
		public string? Avatar { get; set; }
	}
}