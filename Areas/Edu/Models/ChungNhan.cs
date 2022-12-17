#region

using System.ComponentModel.DataAnnotations;
using Api.Areas.Edu.Interfaces;

#endregion

namespace Api.Areas.Edu.Models;

public class ChungNhan : IMetadata, IChungNhan
{
	[Key] public Guid Id { get; set; }
	public string Ten { get; set; } = string.Empty;

	public string NoiDung { get; set; } = string.Empty;
	public Guid? IdMonHoc { get; set; }

	public virtual MonHoc? Mon { get; set; }
	public virtual ICollection<HocPhan>? HocPhan { get; set; }

	public DateTime ThoiGianTao { get; set; }

	[Timestamp] public byte[]? RowVersion { get; set; } = null!;
}