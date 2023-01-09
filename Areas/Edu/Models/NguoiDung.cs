#region

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Api.Areas.Edu.Interfaces;

#endregion

namespace Api.Areas.Edu.Models;

/// <summary>
///     Model người dùng
/// </summary>
public class NguoiDung : IMetadata, INguoiDung
{
	[Key]
	public Guid Id { get; set; }

	/// <summary>
	///     Id sơ yếu lý lịch
	/// </summary>
	public Guid? IdSoYeuLyLich { get; set; }

	/// <summary>
	///     Id kiểu người dùng
	/// </summary>
	public Guid? IdKieuNguoiDung { get; set; }

	/// <summary>
	///     Id tài khoản
	/// </summary>
	public Guid? IdTaiKhoan { get; set; }

	public string? Avatar { get; set; }

	/// <summary>
	///     Timestamp
	/// </summary>
	[Timestamp]
	public byte[]? RowVersion { get; set; } = null!;

	[NotMapped]
	public string PhanLoai => GetType().Name;

	/// <summary>
	///     Sơ yếu lý lịch của người dùng
	/// </summary>
	public virtual SoYeuLyLich? SoYeuLyLich { get; set; }

	/// <summary>
	///     Tài khoản của người dùng
	/// </summary>
	public virtual TaiKhoan? TaiKhoan { get; set; } = null!;

	/// <summary>
	///     Danh sách lịch của người dùng
	/// </summary>
	public virtual ICollection<ChiTietLich> ChiTietLich { get; set; } = new List<ChiTietLich>();

	/// <summary>
	///     Trường thông tin người dùng
	/// </summary>
	public virtual ICollection<GiaTriTruongThongTinNguoiDung> GiaTriTruongThongTinNguoiDung { get; set; } = new HashSet<GiaTriTruongThongTinNguoiDung>();

	/// <summary>
	///     Kiểu người dùng
	/// </summary>
	public virtual KieuNguoiDung? KieuNguoiDung { get; set; }
	/// <summary>
	///     Id
	/// </summary>
}