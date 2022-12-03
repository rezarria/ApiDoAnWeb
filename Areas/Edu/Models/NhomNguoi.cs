using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public class NhomNguoi
{
    [Key]
    public Guid Id { get; set; }

    public string? Ten { get; set; }

    public DateTime NgayTao { get; set; }

    public virtual ICollection<Nguoi>? Nguoi { get; set; }

    public enum TrangThaiHoatDong { KhongConHoatDong, HoatDong };

    public TrangThaiHoatDong TrangThai { get; set; } = TrangThaiHoatDong.HoatDong;

    public virtual ICollection<Lop>? Lop { get; set; }

    [Timestamp]
    public byte[]? RowVersion { get; set; } = null!;
}