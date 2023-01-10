#region

using Api.Areas.Edu.Interfaces;
using System.ComponentModel.DataAnnotations;

#endregion

namespace Api.Areas.Edu.Models;

/// <summary>
///     Model kiểu người dùng
/// </summary>
public class KieuNguoiDung : IKieuNguoiDungModel
{
	#region Key

	/// <summary>
	///     Id
	/// </summary>
	[Key]
	public Guid Id { get; set; }

	#endregion

	/// <summary>
	///     Tên kiểu người dùng
	/// </summary>
	[Required]
	public string Ten { get; set; } = string.Empty;

	/// <summary>
	///     Timestamp
	/// </summary>
	[Timestamp]
	public byte[]? RowVersion { get; set; }

	#region Ref

	/// <summary>
	///     Danh sách trường thông tin người dùng
	/// </summary>
	public virtual ICollection<DanhSachTruongThongTinNguoiDungThuocKieuNguoiDung> DanhSachTruongThongTinNguoiDungThuocKieuNguoiDung { get; set; } = new HashSet<DanhSachTruongThongTinNguoiDungThuocKieuNguoiDung>();

	/// <summary>
	///     Danh sách người dùng
	/// </summary>
	public virtual ICollection<NguoiDung> DanhSachNguoiDung { get; set; } = new HashSet<NguoiDung>();

	#endregion
}