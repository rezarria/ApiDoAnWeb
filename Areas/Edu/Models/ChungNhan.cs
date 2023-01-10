#region

using Api.Areas.Edu.Interfaces;
using System.ComponentModel.DataAnnotations;

#endregion

namespace Api.Areas.Edu.Models;

public class ChungNhan : IMetadata, IChungNhan
{
	#region Key

	[Key]
	public Guid Id { get; set; }

	public Guid? IdMonHoc { get; set; }

	#endregion

	public string Ten { get; set; } = string.Empty;

	public string NoiDung { get; set; } = string.Empty;

	public DateTime ThoiGianTao { get; set; }

	[Timestamp]
	public byte[]? RowVersion { get; set; } = null!;

	#region Ref

	public virtual MonHoc? Mon { get; set; }
	public virtual ICollection<HocPhan> HocPhan { get; set; } = new HashSet<HocPhan>();

	#endregion
}