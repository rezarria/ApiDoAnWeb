using System.ComponentModel.DataAnnotations;

namespace Api.Models.SoYeuLyLich;

/// <summary>
/// </summary>
public class ChungMinh
{
    /// <summary>
    /// </summary>
    /// <value></value>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// </summary>
    /// <value></value>
    public string? So { get; set; }

    /// <summary>
    /// </summary>
    /// <value></value>
    public DateTime? CapNgay { get; set; }

    /// <summary>
    /// </summary>
    /// <value></value>
    public string? NoiCap { get; set; }

    /// <summary>
    /// </summary>
    /// <value></value>
    [Timestamp]
    public byte[]? RowVersion { get; set; } = null!;
}