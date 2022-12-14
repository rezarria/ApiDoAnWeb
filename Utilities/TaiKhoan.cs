#region

using Api.Areas.Edu.Models;

#endregion

namespace Api.Utilities;

public static class TaiKhoan
{
	public static void TaoTaiKhoan(this NguoiDung nguoi, string? matKhau = null)
	{
		if (nguoi.SoYeuLyLich is null)
			throw new NullReferenceException("Không có sơ yếu lý lịch");

		if (nguoi.SoYeuLyLich.HoVaTen is null)
			throw new Exception("Chưa đặt tên");

		matKhau ??= string.Format("{0:ddMMyyyy}", nguoi.SoYeuLyLich.NgaySinh) + string.Join("",
																						    nguoi.SoYeuLyLich.HoVaTen.Trim().LoaiBoDau().Split(" ")
																							     .Select(x => string.Concat(x[0].ToString().ToUpper(), x.AsSpan(1)))).ToLower();

		nguoi.TaiKhoan = new Areas.Edu.Models.TaiKhoan
						 {
							 Username = nguoi.SoYeuLyLich.HoVaTen.ToLower().LoaiBoDau().Replace(" ", "") +
									    string.Format("{0:ddMMyyyy}", nguoi.SoYeuLyLich.NgaySinh),
							 MatKhau = MatKhau.MaHoaMatKhau(matKhau)
						 };
	}

	public static async Task<bool> ChuanBiThemAsync(this NguoiDung nguoi,
													CancellationToken cancellationToken = default)
	{
		cancellationToken.ThrowIfCancellationRequested();
		if (nguoi.SoYeuLyLich is not null && nguoi.SoYeuLyLich.NoiOHienNay is not null)
		{
		}

		// nguoi.TaoTaiKhoan();
		// return await context.TaiKhoan.AnyAsync(x => x.TaiKhoanDangNhap == nguoi.TaiKhoan!.TaiKhoanDangNhap,
		//     cancellationToken);
		return await Task.FromResult(false);
	}
}