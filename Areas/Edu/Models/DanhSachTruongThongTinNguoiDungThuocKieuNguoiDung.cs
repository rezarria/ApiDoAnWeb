using System.ComponentModel.DataAnnotations;
using Api.Areas.Edu.Interfaces;

namespace Api.Areas.Edu.Models;


public class DanhSachTruongThongTinNguoiDungThuocKieuNguoiDung : IMetadata, IDanhSachTruongThongTinNguoiDungThuocKieuTaiKhoan
{
    [Key]
    public Guid Id { get; set; }
    public Guid IdKieuNguoiDung { get; set; }
    public Guid IdTruongThongTinNguoiDung { get; set; }
    [Timestamp]
    public byte[]? RowVersion { get; set; }
    
    public virtual KieuNguoiDung? KieuNguoiDung { get; set; }
    public virtual TruongThongTinNguoiDung? TruongThongTinNguoiDung { get; set; }
}