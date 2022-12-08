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

    public int SoBuoi { get; set; }

    public  string? MieuTa { get; set; }

    [DataType(DataType.Date)]
    public  DateTime ThoiGianTao { get; set; }

    public DateTime ThoiGianBatDau { get; set; }
    public DateTime ThoiGianKetThuc { get; set; }

    public virtual NguoiDung? NguoiTao { get; set; }

    [Timestamp]
    public virtual byte[]? RowVersion { get; set; } = null!;
}