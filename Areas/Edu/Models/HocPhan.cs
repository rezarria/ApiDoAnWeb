using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public class HocPhan
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
	public Guid MonId { get; set; }

	/// <summary>
	/// 
	/// </summary>
	[Required]
	public virtual Mon Mon { get; set; } = null!;

	public int SoBuoi { get; set; }

	/// <summary>
	/// 
	/// </summary>
	[DataType(DataType.Date)]
	public DateTime ThoiGianTao { get; set; }

	public virtual Nguoi? NguoiTao { get; set; }

	public virtual ICollection<GiangVien>? GiangVien { get; set; }

	public virtual ICollection<ChungNhan>? ChungNhan { get; set; }

	public virtual ICollection<HocPhan>? HocPhanYeuCau { get; set; }

	/// <summary>
	/// </summary>
	[Timestamp]
	public byte[]? RowVersion { get; set; } = null!;
}
