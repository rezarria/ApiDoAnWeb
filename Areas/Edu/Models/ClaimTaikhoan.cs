using Api.Areas.Edu.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Areas.Edu.Models;

public class ClaimTaikhoan : IMetadata
{
	#region Key

	[Key]
	public Guid Id { get; set; }
	[Required]
	public Guid IdTaiKhoan { get; set; }

	#endregion

	[Required]
	public string Ten { get; set; } = string.Empty;
	public string? GiaTri { get; set; }

	[Timestamp]
	public byte[]? RowVersion { get; set; }

	public virtual TaiKhoan? TaiKhoan { get; set; }
}