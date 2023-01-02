#region

using Api.Areas.Edu.Interfaces;
using System.Linq.Expressions;

#endregion

namespace Api.Areas.Edu.DTOs;

public static class SoYeuLyLich
{
	public class Get : ISoYeuLyLich
	{
		public static readonly Expression<Func<Models.SoYeuLyLich, ISoYeuLyLich>> Expression = soYeuLyLich =>
																								   soYeuLyLich;

		public static readonly Expression<Func<Models.SoYeuLyLich, dynamic>> ExpressionToiThieu = soYeuLyLich =>
																									  new
																									  {
																										  soYeuLyLich.Id,
																										  soYeuLyLich.HoVaTen
																									  };

		public Guid Id { get; set; }
		public string? HoVaTen { get; set; }
		public GioiTinh? GioiTinh { get; set; }
		public DateTime? NgaySinh { get; set; }
		public string? NoiSinh { get; set; }
		public string? NguyenQuan { get; set; }
		public string? ThuongTru { get; set; }
		public string? SoDienThoai { get; set; }
		public string? Email { get; set; }
		public string? DanToc { get; set; }
		public string? TonGiao { get; set; }
		public string? TrinhDoVanHoa { get; set; }
		public string? SoTruong { get; set; }
	}

	public class Post : ISoYeuLyLichInfo
	{
		public string? HoVaTen { get; set; }
		public GioiTinh? GioiTinh { get; set; }
		public DateTime? NgaySinh { get; set; }
		public string? NoiSinh { get; set; }
		public string? NguyenQuan { get; set; }
		public string? ThuongTru { get; set; }
		public string? SoDienThoai { get; set; }
		public string? Email { get; set; }
		public string? DanToc { get; set; }
		public string? TonGiao { get; set; }
		public string? TrinhDoVanHoa { get; set; }
		public string? SoTruong { get; set; }
	}
}