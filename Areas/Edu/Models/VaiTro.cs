using Api.Areas.Edu.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Api.Areas.Edu.Models;

public class VaiTro : IMetadata
{
	[Required]
	public string Ten { get; set; } = string.Empty;
	[Key]
	public Guid Id { get; set; }
	[Timestamp]
	public byte[]? RowVersion { get; set; }
}