#region

using Api.Areas.Edu.Interfaces;
using Api.Areas.Edu.Interfaces.SoYeuLyLich;
using System.ComponentModel.DataAnnotations;

#endregion

namespace Api.Areas.Edu.Models;

/// <summary>
/// </summary>
public class CanCuocCongDan : IMetadata, ICanCuocCongDan
{
	/// <summary>
	///		Id Căn cước công dân
	/// </summary>
	[Key]
	public Guid Id { get; set; }
	
	/// <summary>
	///		Id Sơ yếu lý lịch
	/// </summary>
	public Guid? IdSoYeuLyLich { get; set; }

	/// <summary>
	///		Số căn cước công dân
	/// </summary>
	/// <value></value>
	public string? So { get; set; }

	/// <summary>
	///		Ngầy cấp
	/// </summary>
	/// <value></value>
	public DateTime? CapNgay { get; set; }

	/// <summary>
	///		Nới cấp
	/// </summary>
	/// <value></value>
	public string? NoiCap { get; set; }

	[Timestamp]
	public byte[]? RowVersion { get; set; } = null!;

	public virtual SoYeuLyLich? SoYeuLyLich { get; set; }
}