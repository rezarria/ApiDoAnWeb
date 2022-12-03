using System.ComponentModel.DataAnnotations;

namespace Api.Models;

/// <summary>
/// </summary>
public class Lop
{
    /// <summary>
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Required] public string Ten { get; set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public int SoBuoi { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [DataType(DataType.Date)]
    public DateTime ThoiGianBatDau { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [DataType(DataType.Date)]
    public DateTime ThoiGianKetThuc { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public enum TrangThaiLopHoc
    {
        /// <summary>
        /// 
        /// </summary>
        Chua,
        /// <summary>
        /// 
        /// </summary>
        Dang,
        /// <summary>
        /// 
        /// </summary>
        Xong
    };

    /// <summary>
    /// 
    /// </summary>
    public TrangThaiLopHoc TrangThai { get; set; }

    /// <summary>
    /// </summary>
    public virtual HocPhan? HocPhan { get; set; }

    /// <summary>
    /// </summary>
    public virtual ICollection<HocVien>? HocVien { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public virtual ICollection<NhomNguoi>? Nhom { get; set; }

    /// <summary>
    /// </summary>
    public virtual ICollection<GiangVien>? GiangVien { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public virtual ICollection<Lich>? Lich { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public virtual ICollection<PhongHoc>? PhongHoc { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public virtual ICollection<NhiemVuHoc>? NhiemVuHoc { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public virtual Nguoi? NguoiTao { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Timestamp]
    public byte[]? RowVersion { get; set; } = null!;
}