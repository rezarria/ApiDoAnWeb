#region

using Api.Areas.Edu.Interfaces;
using System.Linq.Expressions;

#endregion

namespace Api.Areas.Edu.DTOs;

public static class DanhSachTruongThongTinNguoiDungThuocKieuNguoiDung
{
	public class TruongThongTinNguoiDung : ITruongThongTinNguoiDungInfo, IMetadataKey
	{
		public static readonly
			Expression<Func<Models.DanhSachTruongThongTinNguoiDungThuocKieuNguoiDung, TruongThongTinNguoiDung>>
			Expression = danhSach => new TruongThongTinNguoiDung
									 {
										 Id = danhSach.TruongThongTinNguoiDung!.Id,
										 Ten = danhSach.TruongThongTinNguoiDung.Ten ?? string.Empty,
										 KieuDuLieu = danhSach.TruongThongTinNguoiDung.KieuDuLieu
									 };

		public Guid Id { get; set; }
		public string? Ten { get; set; }
		public string KieuDuLieu { get; set; } = string.Empty;
		public string? Alias { get; set; }
	}
}