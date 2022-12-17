#region

using System.Linq.Expressions;
using Api.Areas.Edu.Interfaces;

#endregion

namespace Api.Areas.Edu.DTOs;

public class LopHoc
{
	public class Get : ILopHoc
	{
		public Guid Id { get; set; }
		public string Ten { get; set; } = string.Empty;
		public int SoBuoi { get; set; }
		public DateTime ThoiGianBatDau { get; set; }
		public DateTime ThoiGianKetThuc { get; set; }
		public ILopHoc.TrangThaiLopHoc TrangThai { get; set; }
		public Guid IdHocPhan { get; set; }

		public static Expression<Func<Models.LopHoc, Get>> Expression = lop => new Get
		{
			Id = lop.Id,
			Ten = lop.Ten,
			SoBuoi = lop.SoBuoi,
			ThoiGianBatDau = lop.ThoiGianBatDau,
			ThoiGianKetThuc = lop.ThoiGianKetThuc,
			TrangThai = lop.TrangThai,
			IdHocPhan = lop.IdHocPhan
		};
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