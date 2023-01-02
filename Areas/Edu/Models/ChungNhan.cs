#region

using Api.Areas.Edu.Interfaces;
using System.ComponentModel.DataAnnotations;

#endregion

namespace Api.Areas.Edu.Models;

public class ChungNhan : IMetadata, IChungNhan
{
	public virtual MonHoc? Mon { get; set; }
	public virtual ICollection<HocPhan>? HocPhan { get; set; }
	public string Ten { get; set; } = string.Empty;

	public string NoiDung { get; set; } = string.Empty;
	public Guid? IdMonHoc { get; set; }

	public DateTime ThoiGianTao { get; set; }
	[Key]
	public Guid Id { get; set; }

	[Timestamp]
	public byte[]? RowVersion { get; set; } = null!;
}