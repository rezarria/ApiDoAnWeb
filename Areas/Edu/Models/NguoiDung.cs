using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Api.Areas.Edu.Interfaces;

namespace Api.Areas.Edu.Models;

/// <summary>
/// Model người dùng
/// </summary>
public class NguoiDung : IMetadata, INguoiDung
{
	/// <summary>
	/// Id
	/// </summary>
	[Key]
	public Guid Id { get; set; }

	/// <summary>
	/// Id sơ yếu lý lịch
	/// </summary>
	public Guid? IdSoYeuLyLich { get; set; }

	/// <summary>
	/// Id kiểu người dùng
	/// </summary>
	public Guid? IdKieuNguoiDung {get;set;}

	/// <summary>
	/// Sơ yếu lý lịch của người dùng
	/// </summary>
	[ForeignKey(nameof(IdSoYeuLyLich))]
	public virtual SoYeuLyLich? SoYeuLyLich { get; set; }

	/// <summary>
	/// Tài khoản của người dùng
	/// </summary>
	public virtual TaiKhoan? TaiKhoan { get; set; } = null!;
	
	/// <summary>
	/// Danh sách lịch của người dùng
	/// </summary>
	public virtual ICollection<ChiTietLich> ChiTietLich { get; set; } = new List<ChiTietLich>();

	/// <summary>
	/// Trường thông tin người dùng
	/// </summary>
	public virtual ICollection<GiaTriTruongThongTinNguoiDung> GiaTriTruongThongTinNguoiDung { get; set; } = new List<GiaTriTruongThongTinNguoiDung>();

	/// <summary>
	/// Timestamp
	/// </summary>
	[Timestamp]
	public byte[]? RowVersion { get; set; } = null!;

	[NotMapped]
	public string PhanLoai => GetType().Name;

}