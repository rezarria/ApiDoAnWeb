using System.ComponentModel.DataAnnotations;

namespace Api.Models;

/// <summary>
///     Lịch
/// </summary>
public class Lich
{
    /// <summary>
    ///     Enum tình trạng
    /// </summary>
    public enum TinhTrang
    {
        /// <summary>
        ///     Chưa bắt đầu
        /// </summary>
        ChuaBatDau,

        /// <summary>
        ///     Đang tiếp diễn
        /// </summary>
        DangTiepDien,

        /// <summary>
        ///     Đã xong
        /// </summary>
        DaXong,

        /// <summary>
        ///     Đã huỷ
        /// </summary>
        DaHuy
    }

    /// <summary>
    ///     Guid
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    ///     Thời gian lịch bắt đầu
    /// </summary>
    public DateTime ThoiGianBatDau { get; set; }

    /// <summary>
    ///     Thời gian lịch kết thúc
    /// </summary>
    public DateTime ThoiGianKetThuc { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public string? MoTa { get; set; }

    /// <summary>
    ///     Tình trạng
    /// </summary>
    public TinhTrang TinhTrangLich { get; set; } = TinhTrang.ChuaBatDau;

    public virtual LopHoc? Lop { get; set; }

    public virtual CaHoc? CaHoc { get; set; }

    public virtual PhongHoc? PhongHoc { get; set; }

    public virtual ICollection<ChiTietLich>? ChiTietLich { get; set; }

    /// <summary>
    /// </summary>
    [Timestamp]
    public byte[]? RowVersion { get; set; } = null!;
}