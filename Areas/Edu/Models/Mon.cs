using System.ComponentModel.DataAnnotations;
using Api.Areas.Edu.Interfaces;

namespace Api.Areas.Edu.Models;

/// <summary>
/// Model Môn học
/// </summary>
public class MonHoc : IMetadata, IMonHoc
{
    /// <summary>
    /// Id
    /// </summary>
    [Key]
    public Guid Id { get; set; }
    
    /// <summary>
    /// Tên
    /// </summary>
    [Required]
    public string Ten { get; set; } = null!;

    /// <summary>
    /// Miêu tả
    /// </summary>
    public  string? MieuTa { get; set; }
    
    /// <summary>
    /// Người tạo
    /// </summary>
    public virtual NguoiDung? NguoiTao { get; set; }
    
    /// <summary>
    /// Học phần
    /// </summary>
    public virtual ICollection<HocPhan> HocPhan { get; } = new List<HocPhan>();

    [Timestamp]
    public virtual byte[]? RowVersion { get; set; } = null!;
}