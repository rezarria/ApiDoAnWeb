using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Api.Areas.Edu.Interfaces;

namespace Api.Areas.Edu.Models;

/// <summary>
/// </summary>
public class NguoiDung : IMetadata, INguoiDung
{
    /// <summary>
    /// </summary>
    /// <returns></returns>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// </summary>
    /// <value></value>
    public Guid? SoYeuLyLichId { get; set; }

    /// <summary>
    /// </summary>
    /// <value></value>
    [ForeignKey(nameof(SoYeuLyLichId))]
    public virtual SoYeuLyLich? SoYeuLyLich { get; set; }

    public virtual TaiKhoan? TaiKhoan { get; set; } = null!;

    [NotMapped] public string PhanLoai => GetType().Name;

    public virtual ICollection<ChiTietLich>? ChiTietLich { get; set; }

    /// <summary>
    /// </summary>
    /// <value></value>
    [Timestamp]
    public byte[]? RowVersion { get; set; } = null!;
}