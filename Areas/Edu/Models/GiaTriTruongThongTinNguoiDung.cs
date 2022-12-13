using Api.Areas.Edu.Interfaces;
using Microsoft.Build.Framework;

namespace Api.Areas.Edu.Models;

public class GiaTriTruongThongTinNguoiDung : IGiaTriTruongThongTinNguoiDungModel
{
    public Guid Id { get; set; }

    [Required]
    public Guid IdNguoiDung { get; set; }

    [Required]
    public Guid IdTruongThongTinNguoiDung { get; set; }

    public string? GiaTri { get; set; }
    public NguoiDung NguoiDung { get; set; } = null!;
    public TruongThongTinNguoiDung TruongThongTinNguoiDung { get; set; } = null!;
    public byte[]? RowVersion { get; set; }
}