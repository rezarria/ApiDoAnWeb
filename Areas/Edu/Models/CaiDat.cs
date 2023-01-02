#region

using System.ComponentModel.DataAnnotations;

#endregion

namespace Api.Areas.Edu.Models;

public class CaiDat
{
	[Key]
	public Guid Id { get; set; }
	[Required]
	public string Key { get; set; } = null!;
	public string? Value { get; set; }
	public DateTime ThoiGianCapNhat { get; set; }

	[Timestamp]
	public byte[]? RowVersion { get; set; } = null!;
}