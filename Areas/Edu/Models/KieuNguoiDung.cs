using System.Collections;
using System.ComponentModel.DataAnnotations;
using Api.Areas.Edu.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Api.Areas.Edu.Models;

public class KieuNguoiDung : IKieuNguoiDungModel
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string Ten { get; set; } = string.Empty;
	[ValidateNever]

    public ICollection<TruongThongTinNguoiDung> TruongThongTin { get; set; } = new List<TruongThongTinNguoiDung>();

    [Timestamp]
    public byte[]? RowVersion { get; set; }
}