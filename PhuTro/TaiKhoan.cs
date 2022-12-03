using Api.Contexts;
using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.PhuTro;

public static class TaiKhoan
{
	public static void TaoTaiKhoan(this Nguoi nguoi, string? matKhau = null)
	{
		if (nguoi.SoYeuLyLich is null)
			throw new NullReferenceException("Không có sơ yếu lý lịch");

		if (nguoi.SoYeuLyLich.HoVaTen is null)
			throw new Exception("Chưa đặt tên");

		matKhau ??= string.Format("{0:ddMMyyyy}", nguoi.SoYeuLyLich.SinhNgay) + string.Join("",
			nguoi.SoYeuLyLich.HoVaTen.Trim().LoaiBoDau().Split(" ")
				.Select(x => x[0].ToString().ToUpper() + x.Substring(1))).ToLower();

		nguoi.TaiKhoan = new Models.TaiKhoan
		{
			TaiKhoanDangNhap = nguoi.SoYeuLyLich.HoVaTen.ToLower().LoaiBoDau().Replace(" ", "") +
							   string.Format("{0:ddMMyyyy}", nguoi.SoYeuLyLich.SinhNgay),
			MatKhau = MatKhau.MaHoaMatKhau(matKhau)
		};
	}

	public static async Task<bool> ChuanBiThemAsync(this Nguoi nguoi, AppDbContext context,
		CancellationToken cancellationToken = default)
	{
		cancellationToken.ThrowIfCancellationRequested();
		if (nguoi.SoYeuLyLich is not null && nguoi.SoYeuLyLich.ChoOHienNay is not null)
		{
			if (nguoi.SoYeuLyLich.ChoOHienNay.Tinh is not null)
				context.Entry(nguoi.SoYeuLyLich.ChoOHienNay.Tinh).State = EntityState.Unchanged;
			if (nguoi.SoYeuLyLich.ChoOHienNay.QuanHuyen is not null)
				context.Entry(nguoi.SoYeuLyLich.ChoOHienNay.QuanHuyen).State = EntityState.Unchanged;
		}

		// nguoi.TaoTaiKhoan();
		// return await context.TaiKhoan.AnyAsync(x => x.TaiKhoanDangNhap == nguoi.TaiKhoan!.TaiKhoanDangNhap,
		//     cancellationToken);
		return await Task.FromResult(false);
	}
}