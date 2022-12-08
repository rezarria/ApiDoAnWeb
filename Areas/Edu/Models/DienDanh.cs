using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Api.Areas.Edu.Interfaces;

namespace Api.Areas.Edu.Models;

public class DiemDanh : IMetadata, IDiemDanh
{
    [Key] public Guid Id { get; set; }

    public IDiemDanh.TrangThaiDiemDanh TrangThai { get; set; } = IDiemDanh.TrangThaiDiemDanh.Vang;

    public string? NhanXet { get; set; }

    public DateTime ThoiDiemDiemDanh { get; set; } = DateTime.Now;

    [Timestamp] public byte[]? RowVersion { get; set; } = null!;

    public Guid? IdChiTietLich { get; set; }

    [ForeignKey(nameof(IdChiTietLich))]
    public virtual ChiTietLich? ChiTietLich { get; set; }
}