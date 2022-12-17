﻿#region

using System.ComponentModel.DataAnnotations;
using Api.Areas.Edu.Interfaces;

#endregion

namespace Api.Areas.Edu.Models;

/// <summary>
///     Lịch
/// </summary>
public class Lich : IMetadata, ILich
{
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
    /// </summary>
    public string? MoTa { get; set; }

    /// <summary>
    ///     Tình trạng
    /// </summary>
    public ILich.TinhTrang TinhTrangLich { get; set; } = ILich.TinhTrang.ChuaBatDau;

	public virtual LopHoc? Lop { get; set; }

	public virtual PhongHoc? PhongHoc { get; set; }

	public virtual ICollection<ChiTietLich> ChiTietLich { get; set; } = null!;

    /// <summary>
    /// </summary>
    [Timestamp]
	public byte[]? RowVersion { get; set; } = null!;
}