using System.Collections;
using System.ComponentModel.DataAnnotations;
using Api.Areas.Edu.Interfaces;

namespace Api.Areas.Edu.Models;

public class KieuNguoiDung : IKieuNguoiDungModel
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string Ten { get; set; } = string.Empty;

    public ICollection<TruongThongTinNguoiDung> TruongThongTin { get; set; } = new List<TruongThongTinNguoiDung>();

    [Timestamp]
    public byte[]? RowVersion { get; set; }
}