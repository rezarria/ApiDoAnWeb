using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public class NhiemVuHoc
{
    [Key]
    public Guid Id { get; set; }
    public virtual Nguoi? Nguoi { get; set; }
    public virtual NhomNguoi? Nhom { get; set; }

    public virtual LopHoc? Lop { get; set; }

    public string? NoiDung { get; set; }

    public string? NhanXet { get; set; }

    public DateTime NgayTao { get; set; }

    public enum TrangThaiNhiemVu { ChuaKichHoat, DangKichHoat, DaKichHoat };

    public TrangThaiNhiemVu TrangThai { get; set; } = TrangThaiNhiemVu.ChuaKichHoat;

    [Timestamp]
    public byte[]? RowVersion { get; set; } = null!;
}