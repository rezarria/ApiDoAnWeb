#region

using Api.Areas.Edu.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace Api.Areas.Edu.Models;

/// <summary>
///     Model tài khoản
/// </summary>
public class TaiKhoan : IMetadata, ITaiKhoan
{
	#region Key

	/// <summary>
	///     Id
	/// </summary>
	[Key]
	public Guid Id { get; set; }

	#endregion

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
	///     Timestamp
	/// </summary>
	[Timestamp]
	public byte[]? RowVersion { get; set; } = null!;

	#region Ref

	/// <summary>
	///     Danh sách claim
	/// </summary>
	public virtual List<ClaimTaikhoan> Claims { get; set; } = new();

	/// <summary>
	///     Người dùng
	/// </summary>
	public virtual NguoiDung? NguoiDung { get; set; } = null!;

	#endregion
}