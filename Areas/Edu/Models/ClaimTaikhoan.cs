using Api.Areas.Edu.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Areas.Edu.Models;

public class ClaimTaikhoan : IMetadata
{
	[Required]
	public Guid IdTaiKhoan { get; set; }
	[Required]
	public string Ten { get; set; } = string.Empty;
	public string? GiaTri { get; set; }
	[ForeignKey(nameof(IdTaiKhoan))]
	public virtual TaiKhoan? TaiKhoan { get; set; }
	[Key]
	public Guid Id { get; set; }
	[Timestamp]
	public byte[]? RowVersion { get; set; }
}