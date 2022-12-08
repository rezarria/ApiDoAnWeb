using System.ComponentModel.DataAnnotations;
using Api.Areas.Edu.Interfaces;

namespace Api.Areas.Edu.Models;

/// <summary>
///     Lịch
/// </summary>
public class Lich : IMetadata, ILich
{
    /// <summary>
    ///     Guid
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    ///     Thời gian lịch bắt đầu
    /// </summary>
    public DateTime ThoiGianBatDau { get; set; }

    /// <summary>
    ///     Thời gian lịch kết thúc
    /// </summary>
    public DateTime ThoiGianKetThuc { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? MoTa { get; set; }

    /// <summary>
    ///     Tình trạng
    /// </summary>
    public ILichInfo.TinhTrang TinhTrangLich { get; set; } = ILichInfo.TinhTrang.ChuaBatDau;

    public virtual ILopHoc? Lop { get; set; }

    public virtual IPhongHoc? PhongHoc { get; set; }

    public virtual ICollection<IChiTietLich> ChiTietLich { get; set; } = null!;

    /// <summary>
    /// </summary>
    [Timestamp]
    public byte[]? RowVersion { get; set; } = null!;
}