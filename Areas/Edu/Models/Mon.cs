using System.ComponentModel.DataAnnotations;
using Api.Areas.Edu.Interfaces;

namespace Api.Areas.Edu.Models;

/// <summary>
/// </summary>
public class MonHoc : IMetadata, IMonHoc
{
    /// <summary>
    /// </summary>
    /// <returns></returns>
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string Ten { get; set; } = null!;

    public  string? MieuTa { get; set; }

    public virtual NguoiDung? NguoiTao { get; set; }

    [Timestamp]
    public virtual byte[]? RowVersion { get; set; } = null!;
}