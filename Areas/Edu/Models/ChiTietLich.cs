#region

using Api.Areas.Edu.Interfaces;
using System.ComponentModel.DataAnnotations;

#endregion

namespace Api.Areas.Edu.Models;

public class ChiTietLich : IChiTietLich
{
	#region Key

	[Key]
	public Guid Id { get; set; }

	public Guid IdLich { get; set; }

	public Guid IdNguoiDung { get; set; }
	public Guid? IdNguoiThaoTac { get; set; }

	#endregion

	public virtual Lich? Lich { get; set; }
	public virtual NguoiDung? NguoiDung { get; set; }

	public virtual DiemDanh? DiemDanh { get; set; }

	public virtual NguoiDung? NguoiThaoTac { get; set; }

	[Timestamp]
	public byte[]? RowVersion { get; set; }
}