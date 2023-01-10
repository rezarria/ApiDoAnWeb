using Api.Areas.Edu.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Api.Areas.Edu.Models;

public class VaiTro : IMetadata
{
	#region Key

	[Key]
	public Guid Id { get; set; }

	#endregion

	[Required]
	public string Ten { get; set; } = string.Empty;

	[Timestamp]
	public byte[]? RowVersion { get; set; }

	#region Ref

	public virtual ICollection<NguoiDung> NguoiDung { get; set; } = new HashSet<NguoiDung>();

	#endregion
}