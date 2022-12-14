using System.Collections;
using System.ComponentModel.DataAnnotations;
using Api.Areas.Edu.Interfaces;

namespace Api.Areas.Edu.Models;

public class TruongThongTinNguoiDung : ITruongThongTinNguoiDungInfoModel
{
	[Key]
	public Guid Id { get; set; }
	public string? Ten { get; set; }

	[Required(AllowEmptyStrings = false)]
	public string KieuDuLieu { get; set; } = null!;

	public virtual ICollection<DanhSachTruongThongTinNguoiDungThuocKieuNguoiDung> DanhSachTruongThongTinNguoiDungThuocKieuNguoiDung { get; set; } = new List<DanhSachTruongThongTinNguoiDungThuocKieuNguoiDung>();

	public virtual ICollection<GiaTriTruongThongTinNguoiDung> GiaTri { get; set; } = new List<GiaTriTruongThongTinNguoiDung>();

	[Timestamp]
	public byte[]? RowVersion { get; set; }
}