#region

using System.Linq.Expressions;
using Api.Areas.Edu.Interfaces;

#endregion

namespace Api.Areas.Edu.DTOs;

public static class DanhSachTruongThongTinNguoiDungThuocKieuNguoiDung
{
	public class TruongThongTinNguoiDung : ITruongThongTinNguoiDungInfo, IMetadataKey
	{
		public Guid Id { get; set; }
		public string? Ten { get; set; }
		public string KieuDuLieu { get; set; } = string.Empty;
		public string? Alias { get; set; }

		public static readonly
			Expression<Func<Models.DanhSachTruongThongTinNguoiDungThuocKieuNguoiDung, TruongThongTinNguoiDung>>
			Expression = danhSach => new()
			{
				Id = danhSach.TruongThongTinNguoiDung!.Id,
				Ten = danhSach.TruongThongTinNguoiDung.Ten ?? string.Empty,
				KieuDuLieu = danhSach.TruongThongTinNguoiDung.KieuDuLieu
			};
	}
}