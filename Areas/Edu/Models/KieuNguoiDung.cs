using System.Collections;
using System.ComponentModel.DataAnnotations;
using Api.Areas.Edu.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Api.Areas.Edu.Models;

/// <summary>
/// Model kiểu người dùng
/// </summary>
public class KieuNguoiDung : IKieuNguoiDungModel
{
	/// <summary>
	/// Id
	/// </summary>
	[Key]
	public Guid Id { get; set; }

	/// <summary>
	/// Tên kiểu người dùng 
	/// </summary>
	[Required]
	public string Ten { get; set; } = string.Empty;

	/// <summary>
	/// Danh sách trường thông tin người dùng
	/// </summary>
	[ValidateNever]
	public virtual ICollection<DanhSachTruongThongTinNguoiDungThuocKieuNguoiDung> DanhSachTruongThongTinNguoiDungThuocKieuNguoiDung { get; set; } = new List<DanhSachTruongThongTinNguoiDungThuocKieuNguoiDung>();

	/// <summary>
	/// Timestamp 
	/// </summary>
	[Timestamp]
	public byte[]? RowVersion { get; set; }
}