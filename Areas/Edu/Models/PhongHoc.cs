#region

using Api.Areas.Edu.Interfaces;
using System.ComponentModel.DataAnnotations;

#endregion

namespace Api.Areas.Edu.Models;

/// <summary>
///     Model phòng học
/// </summary>
public class PhongHoc : IMetadata, IPhongHoc
{
	#region Key

	/// <summary>
	///     Id
	/// </summary>
	[Key]
	public Guid Id { get; set; }

	#endregion

	/// <summary>
	///     Tên phòng học
	/// </summary>
	[Required(AllowEmptyStrings = false)]
	public string Ten { get; set; } = string.Empty;


	/// <summary>
	///     Vị trí phòng học
	/// </summary>
	public string ViTri { get; set; } = string.Empty;

	/// <summary>
	///     Timestamp
	/// </summary>
	[Timestamp]
	public byte[]? RowVersion { get; set; } = null!;

	#region Ref

	/// <summary>
	///     Danh sách lớp học sử dụng phòng học
	/// </summary>
	public virtual ICollection<LopHoc> Lop { get; set; } = new HashSet<LopHoc>();

	/// <summary>
	///     Danh sách lịch sử dụng lớp học
	/// </summary>
	public virtual ICollection<Lich> Lich { get; set; } = new HashSet<Lich>();

	#endregion
}