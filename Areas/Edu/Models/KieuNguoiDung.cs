using System.Collections;
using System.ComponentModel.DataAnnotations;
using Api.Areas.Edu.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Api.Areas.Edu.Models;

public class KieuNguoiDung : IKieuNguoiDungModel
{
	[Key]
	public Guid Id { get; set; }

	[Required]
	public string Ten { get; set; } = string.Empty;
	[ValidateNever]

	public virtual ICollection<DanhSachTruongThongTinNguoiDungThuocKieuNguoiDung> DanhSachTruongThongTinNguoiDungThuocKieuNguoiDung { get; set; } = new List<DanhSachTruongThongTinNguoiDungThuocKieuNguoiDung>();

	[Timestamp]
	public byte[]? RowVersion { get; set; }
}