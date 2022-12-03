using System.ComponentModel.DataAnnotations;

namespace Api.Models.SoYeuLyLich;

/// <summary>
/// </summary>
public class QuaTrinhDaoTao
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
    public string? CoSoDaoTao { get; set; }

    /// <summary>
    /// </summary>
    /// <value></value>
    public string? HinhThucDaoTao { get; set; }

    /// <summary>
    /// </summary>
    /// <value></value>
    public string? VanBangChungChi { get; set; }


    /// <summary>
    /// </summary>
    /// <value></value>
    [Timestamp]
    public byte[]? RowVersion { get; set; } = null!;
}