#region

using Api.Areas.Edu.Interfaces;
using System.Linq.Expressions;

#endregion

namespace Api.Areas.Edu.DTOs;

public class LopHoc
{
	public class Get : ILopHoc
	{
		public static readonly Expression<Func<Models.LopHoc, Get>> Expression = lop => new Get
																					    {
																						    Id = lop.Id,
																						    Ten = lop.Ten,
																						    SoBuoi = lop.SoBuoi,
																						    ThoiGianBatDau = lop.ThoiGianBatDau,
																						    ThoiGianKetThuc = lop.ThoiGianKetThuc,
																						    TrangThai = lop.TrangThai,
																						    IdHocPhan = lop.IdHocPhan
																					    };

		public Guid Id { get; set; }
		public string Ten { get; set; } = string.Empty;
		public int SoBuoi { get; set; }
		public DateTime ThoiGianBatDau { get; set; }
		public DateTime ThoiGianKetThuc { get; set; }
		public ILopHoc.TrangThaiLopHoc TrangThai { get; set; }
		public Guid IdHocPhan { get; set; }
	}

	public class Post : ILopHoc
	{
		public string Ten { get; set; } = string.Empty;
		public int SoBuoi { get; set; }
		public DateTime ThoiGianBatDau { get; set; } = DateTime.Now;
		public DateTime ThoiGianKetThuc { get; set; } = DateTime.Now;
		public ILopHoc.TrangThaiLopHoc TrangThai { get; set; } = ILopHoc.TrangThaiLopHoc.Chua;
		public Guid IdHocPhan { get; set; }
	}
}