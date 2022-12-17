using System.Linq.Expressions;
using Api.Areas.Edu.Interfaces;

namespace Api.Areas.Edu.DTOs;

public static class SoYeuLyLich
{
    public class Get : ISoYeuLyLich
    {
        public Guid Id { get; set; }
        public string? HoVaTen { get; set; }
        public GioiTinh? GioiTinh { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string? NoiSinh { get; set; }
        public string? NguyenQuan { get; set; }
        public string? ThuongTru { get; set; }
        public string? SoDienThoai { get; set; }
        public string? Email { get; set; }
        public string? DanToc { get; set; }
        public string? TonGiao { get; set; }
        public string? TrinhDoVanHoa { get; set; }
        public DateTime? TNCS_NgayKetNap { get; set; }
        public string? TNCS_NoiKetNap { get; set; }
        public DateTime? CSVN_NgayKetNap { get; set; }
        public string? CSVN_NoiKetNap { get; set; }
        public string? KhenThuong_KyLuat { get; set; }
        public string? SoTruong { get; set; }

        public static readonly Expression<Func<Models.SoYeuLyLich, ISoYeuLyLich>> Expression = soYeuLyLich =>
            soYeuLyLich;

        public static readonly Expression<Func<Models.SoYeuLyLich, dynamic>> ExpressionToiThieu = soYeuLyLich =>
            new
            {
                soYeuLyLich.Id,
                soYeuLyLich.HoVaTen
            };
    }

    public class Post : ISoYeuLyLichInfo
    {
        public string? HoVaTen { get; set; }
        public GioiTinh? GioiTinh { get; set; }
        public DateTime? NgaySinh { get; set; }
        public string? NoiSinh { get; set; }
        public string? NguyenQuan { get; set; }
        public string? ThuongTru { get; set; }
        public string? SoDienThoai { get; set; }
        public string? Email { get; set; }
        public string? DanToc { get; set; }
        public string? TonGiao { get; set; }
        public string? TrinhDoVanHoa { get; set; }
        public DateTime? TNCS_NgayKetNap { get; set; }
        public string? TNCS_NoiKetNap { get; set; }
        public DateTime? CSVN_NgayKetNap { get; set; }
        public string? CSVN_NoiKetNap { get; set; }
        public string? KhenThuong_KyLuat { get; set; }
        public string? SoTruong { get; set; }
    }
}