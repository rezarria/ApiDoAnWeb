using System.ComponentModel.DataAnnotations;
using Api.Areas.Edu.Interfaces;
using Api.Areas.Edu.Interfaces.SoYeuLyLich;

namespace Api.Areas.Edu.Models;

/// <summary>
/// </summary>
public class QuaTrinhCongTac : IMetadata, IQuaTrinhCongTac
{
    /// <summary>
    /// </summary>
    /// <value></value>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// </summary>
    /// <value></value>
    public DateTime? ThoiGianBatDau { get; set; }

    /// <summary>
    /// </summary>
    /// <value></value>
    public DateTime? ThoiGianKetThuc { get; set; }

    /// <summary>
    /// </summary>
    /// <value></value>
    public string? DonViCongTac { get; set; }

    /// <summary>
    /// </summary>
    /// <value></value>
    public string? ChucVu { get; set; }


    /// <summary>
    /// </summary>
    /// <value></value>
    [Timestamp]
    public byte[]? RowVersion { get; set; } = null!;
}