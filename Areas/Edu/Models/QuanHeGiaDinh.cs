using System.ComponentModel.DataAnnotations;
using Api.Areas.Edu.Interfaces;
using Api.Areas.Edu.Interfaces.SoYeuLyLich;

namespace Api.Areas.Edu.Models;

/// <summary>
/// </summary>
public class QuanHeGiaDinh : IMetadata, IQuanHeGiaDinh
{
    /// <summary>
    /// </summary>
    /// <value></value>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// </summary>
    /// <value></value>
    public string? CoQuanCongTac { get; set; }

    /// <summary>
    /// </summary>
    /// <value></value>
    public virtual SoYeuLyLich? SoYeuLyLich { get; set; }

    /// <summary>
    /// </summary>
    /// <value></value>
    [Timestamp]
    public byte[]? RowVersion { get; set; } = null!;
}