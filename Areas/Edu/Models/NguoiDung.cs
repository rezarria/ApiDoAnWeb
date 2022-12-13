using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Api.Areas.Edu.Interfaces;

namespace Api.Areas.Edu.Models;

/// <summary>
/// </summary>
public class NguoiDung : IMetadata, INguoiDung
{
	/// <summary>
	/// </summary>
	/// <returns></returns>
	[Key]
	public Guid Id { get; set; }

	/// <summary>
	/// </summary>
	/// <value></value>
	public Guid? SoYeuLyLichId { get; set; }

	public Guid? KieuNguoiDung {get;set;}

	/// <summary>
	/// </summary>
	/// <value></value>
	[ForeignKey(nameof(SoYeuLyLichId))]
	public virtual SoYeuLyLich? SoYeuLyLich { get; set; }
	public virtual TaiKhoan? TaiKhoan { get; set; } = null!;
	public virtual ICollection<ChiTietLich> ChiTietLich { get; set; } = new List<ChiTietLich>();
	public virtual ICollection<GiaTriTruongThongTinNguoiDung> GiaTriTruongThongTinNguoiDung { get; set; } = new List<GiaTriTruongThongTinNguoiDung>();

	/// <summary>
	/// </summary>
	/// <value></value>
	[Timestamp]
	public byte[]? RowVersion { get; set; } = null!;

	[NotMapped]
	public string PhanLoai => GetType().Name;

}