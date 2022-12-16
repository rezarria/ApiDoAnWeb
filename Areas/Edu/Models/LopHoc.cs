using System.ComponentModel.DataAnnotations;
using Api.Areas.Edu.Interfaces;

namespace Api.Areas.Edu.Models;

/// <summary>
/// Model Lớp học
/// </summary>
public class LopHoc : IMetadata, ILopHoc
{
	/// <summary>
	/// Id
	/// </summary>
	[Key]
	public Guid Id { get; set; }

	/// <summary>
	/// Tên lớp
	/// </summary>
	[Required] public string Ten { get; set; } = null!;

	/// <summary>
	/// Số buổi học
	/// </summary>
	public int SoBuoi { get; set; }

	/// <summary>
	/// Thời gian bắt đầu lớp học
	/// </summary>
	[DataType(DataType.Date)]
	public DateTime ThoiGianBatDau { get; set; }

	/// <summary>
	/// Thời gian kết thúc lớp học
	/// </summary>
	[DataType(DataType.Date)]
	public DateTime ThoiGianKetThuc { get; set; }

	/// <summary>
	/// Trạng thái hiện tại của lớp học
	/// </summary>
	public ILopHoc.TrangThaiLopHoc TrangThai { get; set; }

	/// <summary>
	/// Id học phần của lớp học
	/// </summary>
	public Guid IdHocPhan { get; set; }

	/// <summary>
	/// Học phần của lớp học
	/// </summary>
	public virtual HocPhan? HocPhan { get; set; }

	/// <summary>
	/// Danh sách người tham gia lớp học (học sinh + giảng viên)
	/// </summary>
	public virtual ICollection<NguoiDung> NguoiThamGia { get; set; } = null!;

	/// <summary>
	/// Danh sách lịch của lớp học
	/// </summary>
	public virtual ICollection<Lich> Lich { get; set; } = null!;

	/// <summary>
	/// Danh sách phòng học được sử dụng bởi lớp học
	/// </summary>
	public virtual ICollection<PhongHoc> PhongHoc { get; set; } = null!;

	/// <summary>
	/// Timestamp
	/// </summary>
	[Timestamp]
	public byte[]? RowVersion { get; set; } = null!;
}