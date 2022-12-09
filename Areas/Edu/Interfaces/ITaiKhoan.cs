namespace Api.Areas.Edu.Interfaces;

public interface ITaiKhoan
{
	string TaiKhoanDangNhap { get; set; }
	byte[]? MatKhau { get; set; }
	DateTime ThoiGianTao { get; set; }
	DateTime ThoiGianDangNhapGanNhat { get; set; }
	Guid IdNguoiDung { get; set; }
}