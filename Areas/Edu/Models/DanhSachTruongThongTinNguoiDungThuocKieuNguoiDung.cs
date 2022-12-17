#region

using System.ComponentModel.DataAnnotations;
using Api.Areas.Edu.Interfaces;

#endregion

namespace Api.Areas.Edu.Models;

/// <summary>
///     Model quan hệ giữa Model kiểu người dùng và Model Trường thông tin người dùng
/// </summary>
public class DanhSachTruongThongTinNguoiDungThuocKieuNguoiDung : IMetadata,
	IDanhSachTruongThongTinNguoiDungThuocKieuTaiKhoan
{
	/// <summary>
	///     Id
	/// </summary>
	[Key]
	public Guid Id { get; set; }

	/// <summary>
	///     Id người dùng
	/// </summary>
	public Guid IdKieuNguoiDung { get; set; }

	/// <summary>
	///     Id trường thông tin người dùng
	/// </summary>
	public Guid IdTruongThongTinNguoiDung { get; set; }

	/// <summary>
	///     Timestamp
	/// </summary>
	[Timestamp]
	public byte[]? RowVersion { get; set; }

	/// <summary>
	///     Kiểu người dùng
	/// </summary>
	public virtual KieuNguoiDung? KieuNguoiDung { get; set; }

	/// <summary>
	///     Trường thông tin người dùng
	/// </summary>
	public virtual TruongThongTinNguoiDung? TruongThongTinNguoiDung { get; set; }
}