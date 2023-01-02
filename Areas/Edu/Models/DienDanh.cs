﻿#region

using Api.Areas.Edu.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#endregion

namespace Api.Areas.Edu.Models;

/// <summary>
///     Model điểm danh
/// </summary>
public class DiemDanh : IMetadata, IDiemDanh
{
	/// <summary>
	///     Chi tiết lịch
	/// </summary>
	[ForeignKey(nameof(IdChiTietLich))]
	public virtual ChiTietLich? ChiTietLich { get; set; }

	/// <summary>
	///     Trạng thái
	/// </summary>
	public IDiemDanh.TrangThaiDiemDanh TrangThai { get; set; } = IDiemDanh.TrangThaiDiemDanh.Vang;


	/// <summary>
	///     Nhận xét
	/// </summary>
	public string? NhanXet { get; set; }

	/// <summary>
	///     Thời điểm điểm danh
	/// </summary>
	public DateTime ThoiDiemDiemDanh { get; set; } = DateTime.Now;

	/// <summary>
	///     Id chi tiết lịch
	/// </summary>
	public Guid? IdChiTietLich { get; set; }
	/// <summary>
	///     Id
	/// </summary>
	[Key]
	public Guid Id { get; set; }

	/// <summary>
	///     Timestamp
	/// </summary>
	[Timestamp]
	public byte[]? RowVersion { get; set; } = null!;
}