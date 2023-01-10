#region

using Api.Areas.Edu.Interfaces;
using System.ComponentModel.DataAnnotations;

#endregion

namespace Api.Areas.Edu.Models;

/// <summary>
///     Model học phần
/// </summary>
public class HocPhan : IMetadata, IHocPhan
{
	#region Key

	/// <summary>
	///     Id
	/// </summary>
	[Key]
	public Guid Id { get; set; }

	/// <summary>
	///     Id môn học
	/// </summary>
	[Required]
	public Guid IdMonHoc { get; set; }

	#endregion

	/// <summary>
	///     Tên học phần
	/// </summary>
	[Required]
	public string Ten { get; set; } = null!;

	/// <summary>
	///     Số buổi của học phần
	/// </summary>
	public int SoBuoi { get; set; }

	/// <summary>
	///     Thời gian tạo học phần
	/// </summary>
	[DataType(DataType.Date)]
	public DateTime ThoiGianTao { get; set; }

	/// <summary>
	///     Miêu tả học phần
	/// </summary>
	public string MieuTa { get; set; } = string.Empty;

	/// <summary>
	///     Timestamp
	/// </summary>
	[Timestamp]
	public byte[]? RowVersion { get; set; }

	#region Ref

	/// <summary>
	///     Môn học
	/// </summary>
	public virtual MonHoc? MonHoc { get; set; }

	/// <summary>
	///     Danh sách chứng nhận của học phần
	/// </summary>
	public virtual ICollection<ChungNhan> ChungNhan { get; set; } = new HashSet<ChungNhan>();

	/// <summary>
	///     Những học phần ưu cầu cho học phần này
	/// </summary>
	public virtual ICollection<HocPhan> HocPhanYeuCau { get; set; } = new HashSet<HocPhan>();

	public virtual ICollection<HocPhan> HocPhanPhuThuoc { get; set; } = new HashSet<HocPhan>();

	public virtual ICollection<LopHoc> LopHoc { get; set; } = new HashSet<LopHoc>();

	#endregion
}