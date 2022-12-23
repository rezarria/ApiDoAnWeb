#region

using System.ComponentModel.DataAnnotations;
using Api.Areas.Edu.Interfaces;

#endregion

namespace Api.Areas.Edu.Models;

/// <summary>
///     Model học phần
/// </summary>
public class HocPhan : IMetadata, IHocPhan
{
	/// <summary>
	///     Id
	/// </summary>
	[Key]
	public Guid Id { get; set; }

	/// <summary>
	///     Tên học phần
	/// </summary>
	[Required]
	public string Ten { get; set; } = null!;

	/// <summary>
	///     Miêu tả học phần
	/// </summary>
	public string MieuTa { get; set; } = string.Empty;

	/// <summary>
	///     Id môn học
	/// </summary>
	[Required]
	public Guid IdMonHoc { get; set; }

	/// <summary>
	///     Môn học
	/// </summary>
	[Required]
	public virtual MonHoc MonHoc { get; set; } = null!;

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
	///     Danh sách chứng nhận của học phần
	/// </summary>
	public virtual ICollection<ChungNhan> ChungNhan { get; set; } = new List<ChungNhan>();

	/// <summary>
	///     Những học phần ưu cầu cho học phần này
	/// </summary>
	public virtual ICollection<HocPhan> HocPhanYeuCau { get; set; } = new List<HocPhan>();

	/// <summary>
	///     Timestamp
	/// </summary>
	[Timestamp]
	public byte[]? RowVersion { get; set; } = null!;
}