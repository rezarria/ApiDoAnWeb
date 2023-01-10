#region

using Api.Areas.Edu.Interfaces;
using System.ComponentModel.DataAnnotations;

#endregion

namespace Api.Areas.Edu.Models;

/// <summary>
///     Lịch
/// </summary>
public class Lich : IMetadata, ILich
{
	#region Key

	/// <summary>
	///     Guid
	/// </summary>
	[Key]
	public Guid Id { get; set; }

	public Guid? IdLopHoc { get; set; }

	public Guid? IdPhongHoc { get; set; }

	#endregion

	/// <summary>
	///     Thời gian lịch bắt đầu
	/// </summary>
	public DateTime ThoiGianBatDau { get; set; }

	/// <summary>
	///     Thời gian lịch kết thúc
	/// </summary>
	public DateTime ThoiGianKetThuc { get; set; }

	/// <summary>
	/// </summary>
	public string? MoTa { get; set; }

	/// <summary>
	///     Tình trạng
	/// </summary>
	public ILich.TinhTrang TinhTrangLich { get; set; } = ILich.TinhTrang.ChuaBatDau;

	/// <summary>
	/// </summary>
	[Timestamp]
	public byte[]? RowVersion { get; set; } = null!;

	#region Ref

	public virtual LopHoc? Lop { get; set; }

	public virtual PhongHoc? PhongHoc { get; set; }

	public virtual ICollection<ChiTietLich> ChiTietLich { get; set; } = null!;

	#endregion
}