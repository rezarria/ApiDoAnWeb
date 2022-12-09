using System.ComponentModel.DataAnnotations;
using Api.Areas.Edu.Interfaces;

namespace Api.Areas.Edu.Models;

/// <summary>
/// </summary>
public class SoYeuLyLich : ISoYeuLyLichModel
{
    /// <summary>
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// </summary>
    [Required(ErrorMessage = "Vui lòng không bỏ trống!")]
    public string? HoVaTen { get; set; }

    /// <summary>
    /// </summary>
    public GioiTinh? GioiTinh { get; set; }

    /// <summary>
    /// </summary>
    [Required(ErrorMessage = "Vui lòng nhập ngày sinh")]
    [DisplayFormat(DataFormatString = "dd/MM/yyyy", ApplyFormatInEditMode = true)]
    [DataType(DataType.Date)]
    public DateTime? SinhNgay { get; set; }

    /// <summary>
    /// </summary>
    public string? NoiSinh { get; set; }

    /// <summary>
    /// </summary>
    public string? NguyenQuan { get; set; }

    /// <summary>
    /// </summary>
    public string? NoiDangKyHoKhauThuongTru { get; set; }

    /// <summary>
    /// </summary>
    public string? ChoOHienNay { get; set; }

    /// <summary>
    /// </summary>
    [Phone(ErrorMessage = "Vui lòng nhập đúng định dạng số điện thoại")]
    [Required(ErrorMessage = "Vui lòng cung cấp số điện thoại")]
    [DataType(DataType.PhoneNumber)]
    public string? DienThoai { get; set; }

    /// <summary>
    /// </summary>
    [EmailAddress(ErrorMessage = "Vui lòng nhập đúng định dạng email")]
    [Required(ErrorMessage = "Vui lòng cung cấp địa chỉ email")]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }

    /// <summary>
    /// </summary>
    public string? DanToc { get; set; }

    /// <summary>
    /// </summary>
    public string? TonGiao { get; set; }

    /// <summary>
    /// </summary>
    public virtual CanCuocCongDan? CanCuocCongDan { get; set; }

    /// <summary>
    /// </summary>
    public string? TrinhDoVanHoa { get; set; }

    /// <summary>
    /// </summary>
    public DateTime? TNCS_NgayKetNap { get; set; }

    /// <summary>
    /// </summary>
    public string? TNCS_NoiKetNap { get; set; }

    /// <summary>
    /// </summary>
    public DateTime? CSVN_NgayKetNap { get; set; }

    /// <summary>
    /// </summary>
    public string? CSVN_NoiKetNap { get; set; }

    /// <summary>
    /// </summary>
    public string? KhenThuong_KyLuat { get; set; }

    /// <summary>
    /// </summary>
    public string? SoTruong { get; set; }

    /// <summary>
    /// </summary>
    public virtual ICollection<QuanHeGiaDinh>? QuanHeGiaDinh { get; set; }

    /// <summary>
    /// </summary>
    public virtual ICollection<QuaTrinhDaoTao>? QuaTrinhDaoTao { get; set; }

    /// <summary>
    /// </summary>
    public virtual ICollection<QuaTrinhCongTac>? QuaTrinhCongTac { get; set; }

    /// <summary>
    /// </summary>
    [Timestamp]
    public byte[]? RowVersion { get; set; }
}