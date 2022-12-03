using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Api.Models;

public class DiemDanh
{
    [Key] public Guid Id { get; set; }

    public TrangThaiDiemDanh TrangThai { get; set; } = TrangThaiDiemDanh.Vang;

    public string? NhanXet { get; set; }

    public DateTime ThoiDiemDiemDanh { get; set; } = DateTime.Now;

    [Timestamp] public byte[]? RowVersion { get; set; } = null!;

    public Guid? IdChiTietLich { get; set; }

    [ForeignKey(nameof(IdChiTietLich))]
    public virtual ChiTietLich? ChiTietLich { get; set; }

    public enum TrangThaiDiemDanh
    {
        Vang,
        CoMat,
        CoPhep,
        Muon
    }
}