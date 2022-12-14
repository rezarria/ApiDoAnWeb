#region

using Api.Areas.Edu.Interfaces;
using System.ComponentModel.DataAnnotations;

#endregion

namespace Api.Areas.Edu.Models;

/// <summary>
///     Model trường thông tin người dùng
/// </summary>
public class TruongThongTinNguoiDung : ITruongThongTinNguoiDungInfoModel
{
	#region Key

	/// <summary>
	///     Id
	/// </summary>
	[Key]
	public Guid Id { get; set; }

	#endregion

	/// <summary>
	///     Tên
	/// </summary>
	public string? Ten { get; set; }

	/// <summary>
	///     Alias
	/// </summary>
	public string? Alias { get; set; }

	/// <summary>
	///     Kiểu dữ liệu
	/// </summary>
	/// <value></value>
	[Required(AllowEmptyStrings = false)]
	public string KieuDuLieu { get; set; } = null!;

	/// <summary>
	///     Timestamp
	/// </summary>
	[Timestamp]
	public byte[]? RowVersion { get; set; }

	#region Ref

	/// <summary>
	///     Quan hệ mới người dùng
	/// </summary>
	public virtual ICollection<DanhSachTruongThongTinNguoiDungThuocKieuNguoiDung> DanhSachTruongThongTinNguoiDungThuocKieuNguoiDung { get; set; } = new HashSet<DanhSachTruongThongTinNguoiDungThuocKieuNguoiDung>();

	/// <summary>
	///     Giá trị trường với người dùng
	/// </summary>
	public virtual ICollection<GiaTriTruongThongTinNguoiDung> GiaTri { get; set; } = new HashSet<GiaTriTruongThongTinNguoiDung>();

	#endregion
}