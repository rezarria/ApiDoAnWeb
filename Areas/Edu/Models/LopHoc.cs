using System.ComponentModel.DataAnnotations;
using Api.Areas.Edu.Interfaces;

namespace Api.Areas.Edu.Models;

/// <summary>
/// </summary>
public class LopHoc : IMetadata, ILopHoc
{
	/// <summary>
	/// </summary>
	[Key]
	public Guid Id { get; set; }

	/// <summary>
	/// 
	/// </summary>
	[Required] public string Ten { get; set; } = null!;

	/// <summary>
	/// 
	/// </summary>
	public int SoBuoi { get; set; }

	/// <summary>
	/// 
	/// </summary>
	[DataType(DataType.Date)]
	public DateTime ThoiGianBatDau { get; set; }

	/// <summary>
	/// 
	/// </summary>
	[DataType(DataType.Date)]
	public DateTime ThoiGianKetThuc { get; set; }

	/// <summary>
	/// 
	/// </summary>
	public ILopHoc.TrangThaiLopHoc TrangThai { get; set; }

	/// <summary>
	/// </summary>
	public virtual HocPhan? HocPhan { get; set; }

	/// <summary>
	/// </summary>
	public virtual ICollection<NguoiDung> NguoiThamGia { get; set; } = null!;

	/// <summary>
	/// 
	/// </summary>
	public virtual ICollection<Lich> Lich { get; set; } = null!;

	/// <summary>
	/// 
	/// </summary>
	public virtual ICollection<PhongHoc> PhongHoc { get; set; } = null!;

	/// <summary>
	/// 
	/// </summary>
	public virtual NguoiDung? NguoiTao { get; set; }

	/// <summary>
	/// 
	/// </summary>
	[Timestamp]
	public byte[]? RowVersion { get; set; } = null!;
}