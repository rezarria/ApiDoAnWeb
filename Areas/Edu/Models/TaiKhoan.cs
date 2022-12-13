using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Api.Areas.Edu.Interfaces;

namespace Api.Areas.Edu.Models;

public class TaiKhoan : IMetadata, ITaiKhoan
{
	[Key]
	public Guid Id { get; set; }

	[Required]
	[StringLength(255)]
	public string TaiKhoanDangNhap { get; set; } = null!;

	public byte[]? MatKhau { get; set; }

	public DateTime ThoiGianTao { get; set; } = DateTime.Now;
	public DateTime ThoiGianDangNhapGanNhat { get; set; }

	public Guid IdNguoiDung { get; set; }

	[ForeignKey(nameof(IdNguoiDung))]
	public virtual NguoiDung NguoiDung { get; set; } = null!;

	[Timestamp]
	public byte[]? RowVersion { get; set; } = null!;
}