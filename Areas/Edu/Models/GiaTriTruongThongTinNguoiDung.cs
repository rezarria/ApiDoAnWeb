#region

using Api.Areas.Edu.Interfaces;
using System.ComponentModel.DataAnnotations;

#endregion

namespace Api.Areas.Edu.Models;

/// <summary>
///     Model giá trị trường thông tin người dùng
/// </summary>
public class GiaTriTruongThongTinNguoiDung : IGiaTriTruongThongTinNguoiDungModel
{
	/// <summary>
	///     Người dùng
	/// </summary>
	/// <value></value>
	public NguoiDung? NguoiDung { get; set; }

	/// <summary>
	///     Trường thông tin người dùng
	/// </summary>
	public TruongThongTinNguoiDung? TruongThongTinNguoiDung { get; set; }
	/// <summary>
	///     Id
	/// </summary>
	public Guid Id { get; set; }

	/// <summary>
	///     Id người dùng
	/// </summary>
	/// <value></value>
	[Required]
	public Guid IdNguoiDung { get; set; }

	/// <summary>
	///     Id trường thông tin người dùng
	/// </summary>
	/// <value></value>
	[Required]
	public Guid IdTruongThongTinNguoiDung { get; set; }

	/// <summary>
	///     Giá trị
	/// </summary>
	[Required]
	public string GiaTri { get; set; } = string.Empty;

	/// <summary>
	///     Timestamp
	/// </summary>
	[Timestamp]
	public byte[]? RowVersion { get; set; }
}