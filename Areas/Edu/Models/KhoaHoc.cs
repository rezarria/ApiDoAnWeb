using System.ComponentModel.DataAnnotations;

namespace Api.Models;

/// <summary>
/// </summary>
public class KhoaHoc
{
    /// <summary>
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// </summary>
    [Required]
    public string Ten { get; set; } = null!;

    /// <summary>
    /// </summary>
    public string? MieuTa { get; set; }

    /// <summary>
    /// </summary>
    public virtual ICollection<Mon>? Mon { get; set; }

    /// <summary>
    /// </summary>
    public virtual ICollection<LopHoc>? Lop { get; set; }

    /// <summary>
    /// </summary>
    [Timestamp]
    public byte[]? RowVersion { get; set; } = null!;
}