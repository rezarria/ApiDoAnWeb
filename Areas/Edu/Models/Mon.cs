#region

using Api.Areas.Edu.Interfaces;
using System.ComponentModel.DataAnnotations;

#endregion

namespace Api.Areas.Edu.Models;

/// <summary>
///     Model Môn học
/// </summary>
public class MonHoc : IMetadata, IMonHoc
{
	/// <summary>
	///     Id
	/// </summary>
	[Key]
	public Guid Id { get; set; }

	/// <summary>
	///     Tên
	/// </summary>
	[Required]
	public string Ten { get; set; } = null!;

	/// <summary>
	///     Miêu tả
	/// </summary>
	public string? MieuTa { get; set; }

	[Timestamp]
	public virtual byte[]? RowVersion { get; set; } = null!;

	#region Ref

	/// <summary>
	///     Người tạo
	/// </summary>
	public virtual NguoiDung? NguoiTao { get; set; }

	/// <summary>
	///     Học phần
	/// </summary>
	public virtual ICollection<HocPhan> HocPhan { get; } = new HashSet<HocPhan>();

	public virtual ICollection<ChungNhan> ChungNhan { get; set; } = new HashSet<ChungNhan>();

	#endregion
}