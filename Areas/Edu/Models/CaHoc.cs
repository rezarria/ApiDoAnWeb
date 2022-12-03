using System.ComponentModel.DataAnnotations;

namespace Api.Models;

/// <summary>
/// 
/// </summary>
public class CaHoc
{
    /// <summary>
    /// 
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Required]
    public string Ten { get; set; } = string.Empty;

    /// <summary>
    /// 
    /// </summary>
    [DataType(DataType.Date)]
    public DateTime ThoiGianBatDau;

    /// <summary>
    /// 
    /// </summary>
    [DataType(DataType.Date)]
    public DateTime ThoiGianKetThuc;

    /// <summary>
    /// 
    /// </summary>
    [DataType(DataType.Date)]
    public DateTime ThoiGianTao;

    /// <summary>
    /// 
    /// </summary>
    public virtual Nguoi? NguoiTao { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public virtual ICollection<Lich>? Lich { get; set; }

    [Timestamp]
    public byte[]? RowVersion { get; set; } = null!;
}
