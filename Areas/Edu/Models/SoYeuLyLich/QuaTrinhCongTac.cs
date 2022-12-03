using System.ComponentModel.DataAnnotations;

namespace Api.Models.SoYeuLyLich;

/// <summary>
/// </summary>
public class QuaTrinhCongTac
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