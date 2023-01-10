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
	/// <summary>
	///     Nơi ở hiện nay
	/// </summary>
	public string? NoiOHienNay { get; set; }

	/// <summary>
	///     Căng cước công dân
	/// </summary>
	public virtual CanCuocCongDan? CanCuocCongDan { get; set; }

	public virtual NguoiDung? NguoiDung { get; set; }
	/// <summary>
	///     Id
	/// </summary>
	[Key]
	public Guid Id { get; set; }

	/// <summary>
	///     Họ và tên
	/// </summary>
	[Required(ErrorMessage = "Vui lòng không bỏ trống!")]
	public string? HoVaTen { get; set; }

	/// <summary>
	///     Giới tính
	/// </summary>
	public GioiTinh? GioiTinh { get; set; }

	/// <summary>
	///     Ngày sinh
	/// </summary>
	[Required(ErrorMessage = "Vui lòng nhập ngày sinh")]
	[DisplayFormat(DataFormatString = "dd/MM/yyyy", ApplyFormatInEditMode = true)]
	[DataType(DataType.Date)]
	public DateTime? NgaySinh { get; set; }

	/// <summary>
	///     Nơi sinh
	/// </summary>
	public string? NoiSinh { get; set; }

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
	//[Phone(ErrorMessage = "Vui lòng nhập đúng định dạng số điện thoại")]
	//[Required(ErrorMessage = "Vui lòng cung cấp số điện thoại")]
	//[DataType(DataType.PhoneNumber)]
	public string? SoDienThoai { get; set; }

	/// <summary>
	///     Email
	/// </summary>
	[EmailAddress(ErrorMessage = "Vui lòng nhập đúng định dạng email")]
	[Required(ErrorMessage = "Vui lòng cung cấp địa chỉ email")]
	[DataType(DataType.EmailAddress)]
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
}