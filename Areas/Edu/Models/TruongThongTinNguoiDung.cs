using System.Collections;
using System.ComponentModel.DataAnnotations;
using Api.Areas.Edu.Interfaces;

namespace Api.Areas.Edu.Models;

public class TruongThongTinNguoiDung : ITruongThongTinNguoiDungInfoModel
{
    [Key]
    public Guid Id { get; set; }

    [Required(AllowEmptyStrings = false)]
    public string TieuDe { get; set; } = null!;

    [Required(AllowEmptyStrings = false)]
    public string KieuDuLieu { get; set; } = null!;

    public virtual ICollection<KieuNguoiDung> KieuNguoiDung { get; set; } = new List<KieuNguoiDung>();

    public virtual ICollection<GiaTriTruongThongTinNguoiDung> GiaTri { get; set; } = new List<GiaTriTruongThongTinNguoiDung>();
    
    [Timestamp]
    public byte[]? RowVersion { get; set; }
}