#region

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Api.Areas.Edu.Interfaces;

#endregion

namespace Api.Areas.Edu.Models;

/// <summary>
///     Model tài khoản
/// </summary>
public class TaiKhoan : IMetadata, ITaiKhoan
{
	/// <summary>
	///     Id
	/// </summary>
	[Key]
	public Guid Id { get; set; }

	/// <summary>
	///     Username
	/// </summary>
	[Required]
	[StringLength(255)]
	public string Username { get; set; } = null!;

	/// <summary>
	///     Mật khẩu
	/// </summary>
	public byte[]? MatKhau { get; set; }

	/// <summary>
	///     Thời gian tạo
	/// </summary>
	public DateTime ThoiGianTao { get; set; } = DateTime.Now;

	/// <summary>
	///     Thời gian đăng nhập gần nhất
	/// </summary>
	public DateTime ThoiGianDangNhapGanNhat { get; set; }

	/// <summary>
	///     Id người dùng
	/// </summary>
	public Guid IdNguoiDung { get; set; }

	/// <summary>
	///     Người dùng
	/// </summary>
	[ForeignKey(nameof(IdNguoiDung))]
	public virtual NguoiDung NguoiDung { get; set; } = null!;

	/// <summary>
	///     Timestamp
	/// </summary>
	[Timestamp]
	public byte[]? RowVersion { get; set; } = null!;
}