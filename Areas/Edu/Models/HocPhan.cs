using System.ComponentModel.DataAnnotations;
using Api.Areas.Edu.Interfaces;

namespace Api.Areas.Edu.Models;

public class HocPhan : IMetadata, IHocPhan
{
	/// <summary>
	/// </summary>
	/// <returns></returns>
	[Key]
	public Guid Id { get; set; }

	/// <summary>
	/// </summary>
	[Required]
	public string Ten { get; set; } = null!;

	public string MieuTa { get; set; } = string.Empty;

	[Required]
	public Guid IdMon { get; set; }

	/// <summary>
	/// 
	/// </summary>
	[Required]
	public virtual MonHoc MonHoc { get; set; } = null!;

	public int SoBuoi { get; set; }

	/// <summary>
	/// 
	/// </summary>
	[DataType(DataType.Date)]
	public DateTime ThoiGianTao { get; set; }

	public virtual NguoiDung? NguoiTao { get; set; }

	public virtual ICollection<NguoiDung> NguoThamGia { get; set; } = null!;

	public virtual ICollection<ChungNhan> ChungNhan { get; set; } = null!;

	public virtual ICollection<HocPhan> HocPhanYeuCau { get; set; } = null!;

	/// <summary>
	/// </summary>
	[Timestamp]
	public byte[]? RowVersion { get; set; } = null!;
}
