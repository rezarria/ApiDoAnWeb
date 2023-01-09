namespace Api.Areas.Edu.Interfaces;

public interface INguoiDung
{
	Guid? IdKieuNguoiDung { get; set; }
	Guid? IdTaiKhoan { get; set; }
	Guid? IdSoYeuLyLich { get; set; }
	string? Avatar { get; set; }
	string PhanLoai => GetType().Name;
}