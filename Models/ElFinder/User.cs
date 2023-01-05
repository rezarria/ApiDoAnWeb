using System.ComponentModel.DataAnnotations;

namespace Api.Models.ElFinder;

public class User
{
	[Key]
	public Guid Id { get; set; }
	[Required]
	public string VolumePath { get; set; } = string.Empty;
	public long QuotaInBytes { get; set; } = 1_000_000;
}