using System.ComponentModel.DataAnnotations;

namespace Api.Models.SoYeuLyLich;

/// <summary>
/// </summary>
public enum GioiTinh
{
    /// <summary>Nam</summary>
    Nam,

    /// <summary>Nữ</summary>
    Nu
}

/// <summary>
/// </summary>
public class SoYeuLyLich
{
    /// <summary>
    /// </summary>
    /// <value></value>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// </summary>
    /// <value></value>
    [Required(ErrorMessage = "Vui lòng không bỏ trống!")]
    public string? HoVaTen { get; set; }

    /// <summary>
    /// </summary>
    /// <value></value>
    public GioiTinh? GioiTinh { get; set; }

    /// <summary>
    /// </summary>
    /// <value></value>
    [Required(ErrorMessage = "Vui lòng nhập ngày sinh")]
    [DisplayFormat(DataFormatString = "dd/MM/yyyy", ApplyFormatInEditMode = true)]
    [DataType(DataType.Date)]
    public DateTime? SinhNgay { get; set; }

    /// <summary>
    /// </summary>
    /// <value></value>
    public string? NoiSinh { get; set; }

    /// <summary>
    /// </summary>
    /// <value></value>
    public string? NguyenQuan { get; set; }

    /// <summary>
    /// </summary>
    /// <value></value>
    public string? NoiDangKyHoKhauThuongTru { get; set; }

    /// <summary>
    /// </summary>
    /// <value></value>
    public DiaChi.DiaChi? ChoOHienNay { get; set; }

    /// <summary>
    /// </summary>
    /// <value></value>
    [Phone(ErrorMessage = "Vui lòng nhập đúng định dạng số điện thoại")]
    [Required(ErrorMessage = "Vui lòng cung cấp số điện thoại")]
    [DataType(DataType.PhoneNumber)]
    public string? DienThoai { get; set; }

    /// <summary>
    /// </summary>
    /// <value></value>
    [EmailAddress(ErrorMessage = "Vui lòng nhập đúng định dạng email")]
    [Required(ErrorMessage = "Vui lòng cung cấp địa chỉ email")]
    [DataType(DataType.EmailAddress)]
    public string? Email { get; set; }

    /// <summary>
    /// </summary>
    /// <value></value>
    public string? DanToc { get; set; }

    /// <summary>
    /// </summary>
    /// <value></value>
    public string? TonGiao { get; set; }

    /// <summary>
    /// </summary>
    /// <value></value>
    public virtual ChungMinh? ChungMinh { get; set; }

    /// <summary>
    /// </summary>
    /// <value></value>
    public string? TrinhDoVanHoa { get; set; }

    /// <summary>
    /// </summary>
    /// <value></value>
    public DateTime? TNCS_NgayKetNap { get; set; }

    /// <summary>
    /// </summary>
    /// <value></value>
    public string? TNCS_NoiKetNap { get; set; }

    /// <summary>
    /// </summary>
    /// <value></value>
    public DateTime? CSVN_NgayKetNap { get; set; }

    /// <summary>
    /// </summary>
    /// <value></value>
    public string? CSVN_NoiKetNap { get; set; }

    /// <summary>
    /// </summary>
    /// <value></value>
    public string? KhenThuong_KyLuat { get; set; }

    /// <summary>
    /// </summary>
    /// <value></value>
    public string? SoTruong { get; set; }

    /// <summary>
    /// </summary>
    /// <value></value>
    public virtual ICollection<QuanHeGiaDinh>? QuanHeGiaDinh { get; set; }

    /// <summary>
    /// </summary>
    /// <value></value>
    public virtual ICollection<QuaTrinhDaoTao>? QuaTrinhDaoTao { get; set; }

    /// <summary>
    /// </summary>
    /// <value></value>
    public virtual ICollection<QuaTrinhCongTac>? QuaTrinhCongTac { get; set; }

    /// <summary>
    /// </summary>
    /// <value></value>
    [Timestamp]
    public byte[]? RowVersion { get; set; } = null!;
}