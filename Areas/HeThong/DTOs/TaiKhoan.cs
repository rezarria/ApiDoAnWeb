using Api.Utilities;
using System.ComponentModel.DataAnnotations;
using TaiKhoan=Api.Areas.Edu.Models.TaiKhoan;

namespace Api.Areas.HeThong.DTOs;

public static class Taikhoan
{
	public class YeuCauTaoMoi
	{
		[Required(AllowEmptyStrings = false)]
		public string UserName { get; set; } = string.Empty;
		[Required(AllowEmptyStrings = false)]
		public string Password { get; set; } = string.Empty;

		public TaiKhoan ChuyenDoi()
		{
			TaiKhoan taiKhoan = new();
			taiKhoan.Username = UserName;
			taiKhoan.MatKhau = MatKhau.MaHoaMatKhau(Password);
			return taiKhoan;
		}
	}
}