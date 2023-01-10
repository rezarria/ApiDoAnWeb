#region

using Api.Areas.Edu.Interfaces;
using System.ComponentModel.DataAnnotations;

#endregion

namespace Api.Areas.Edu.Models;

/// <summary>
///     Model sơ yếu lý lịch
/// </summary>
public class SoYeuLyLich : ISoYeuLyLichModel
{
	#region Key

	/// <summary>
	///     Id
	/// </summary>
	[Key]
	public Guid Id { get; set; }

	#endregion

	/// <summary>
	///     Họ và tên
	/// </summary>
	public string? HoVaTen { get; set; }

	/// <summary>
	///     Giới tính
	/// </summary>
	public GioiTinh? GioiTinh { get; set; }

	/// <summary>
	///     Ngày sinh
	/// </summary>
	public DateTime? NgaySinh { get; set; }

	/// <summary>
	///     Nơi sinh
	/// </summary>
	public string? NoiSinh { get; set; }

	/// <summary>
	///     Nơi ở hiện nay
	/// </summary>
	public string? NoiOHienNay { get; set; }

	/// <summary>
	///     Nguyên quán
	/// </summary>
	public string? NguyenQuan { get; set; }

	/// <summary>
	///     Thường trú
	/// </summary>
	public string? ThuongTru { get; set; }

	/// <summary>
	///     Số điện thoại
	/// </summary>
	public string? SoDienThoai { get; set; }

	/// <summary>
	///     Email
	/// </summary>
	public string? Email { get; set; }

	/// <summary>
	///     Dân tộc
	/// </summary>
	public string? DanToc { get; set; }

	/// <summary>
	///     Tôn giáo
	/// </summary>
	public string? TonGiao { get; set; }

	/// <summary>
	///     Trình độ văn hóa
	/// </summary>
	public string? TrinhDoVanHoa { get; set; }

	/// <summary>
	///     Sở trường
	/// </summary>
	public string? SoTruong { get; set; }

	/// <summary>
	///     Timestamp
	/// </summary>
	[Timestamp]
	public byte[]? RowVersion { get; set; }

	#region Ref

	/// <summary>
	///     Căng cước công dân
	/// </summary>
	public virtual CanCuocCongDan? CanCuocCongDan { get; set; }

	/// <summary>
	///		Người dùng
	/// </summary>
	public virtual NguoiDung? NguoiDung { get; set; }

	#endregion
}