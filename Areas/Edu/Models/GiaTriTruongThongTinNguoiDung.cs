using Api.Areas.Edu.Interfaces;
using Microsoft.Build.Framework;

namespace Api.Areas.Edu.Models;

public class GiaTriTruongThongTinNguoiDung : IGiaTriTruongThongTinNguoiDungModel
{
	public Guid Id { get; set; }

	[Required]
	public Guid IdNguoiDung { get; set; }

	[Required]
	public Guid IdTruongThongTinNguoiDung { get; set; }

	[Required]
	public string GiaTri { get; set; } = string.Empty;
	public NguoiDung? NguoiDung { get; set; }
	public TruongThongTinNguoiDung? TruongThongTinNguoiDung { get; set; }
	public byte[]? RowVersion { get; set; }
}