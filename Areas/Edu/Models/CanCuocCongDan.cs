using System.ComponentModel.DataAnnotations;
using Api.Areas.Edu.Interfaces;
using Api.Areas.Edu.Interfaces.SoYeuLyLich;

namespace Api.Areas.Edu.Models;

/// <summary>
/// </summary>
public class CanCuocCongDan : IMetadata, ICanCuocCongDan
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