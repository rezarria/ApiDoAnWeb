namespace Api.Models.Interfaces;

public interface ITaiKhoan : IMetadata, ITaiKhoanInfo, ITaiKhoanRef
{


}

public interface ITaiKhoanInfo
{
    string TaiKhoanDangNhap { get; set; }

    byte[]? MatKhau { get; set; }

    DateTime ThoiGianTao { get; set; }
    DateTime ThoiGianDangNhapGanNhat { get; set; }

    Guid NguoiDungId { get; set; }
}

public interface ITaiKhoanRef
{
    INguoiDung NguoiDung { get; set; }
}
