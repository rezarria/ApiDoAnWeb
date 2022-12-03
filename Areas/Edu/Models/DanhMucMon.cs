using Api.Models.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Api.Models;

/// <summary>
/// </summary>
public class DanhMucMon : IDanhMucMon
{
    /// <summary>
    /// </summary>
    /// <returns></returns>
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string Ten { get; set; } = null!;

    public string? MieuTa { get; set; }

    [DataType(DataType.Date)]
    public DateTime ThoiGianTao { get; set; }

    public virtual ICollection<Mon>? Mon { get; set; }

    public virtual Nguoi? NguoiTao { get; set; }

    [Timestamp]
    public byte[]? RowVersion { get; set; } = null!;
}