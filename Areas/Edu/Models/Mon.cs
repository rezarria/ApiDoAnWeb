using Api.Models.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Api.Models;

/// <summary>
/// </summary>
public class Mon : IMon
{
    /// <summary>
    /// </summary>
    /// <returns></returns>
    [Key]
    public virtual Guid Id { get; set; }

    [Required]
    public virtual string Ten { get; set; } = null!;

    public virtual string? MieuTa { get; set; }

    [DataType(DataType.Date)]
    public virtual DateTime ThoiGianTao { get; set; }

    public virtual Nguoi? NguoiTao { get; set; }

    public virtual ICollection<DanhMucMon>? DanhMucMon { get; set; }

    public virtual ICollection<KhoaHoc>? KhoaHoc { get; set; }

    [Timestamp]
    public virtual byte[]? RowVersion { get; set; } = null!;
}