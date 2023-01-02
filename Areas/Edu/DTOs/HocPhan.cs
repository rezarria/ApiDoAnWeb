#region

using Api.Areas.Edu.Interfaces;
using System.Linq.Expressions;

#endregion

namespace Api.Areas.Edu.DTOs;

public class HocPhan
{
	public class Get : IHocPhan
	{
		public static readonly Expression<Func<Models.HocPhan, Get>> Expression = hocPhan => new Get
																						     {
																							     Id = hocPhan.Id,
																							     Ten = hocPhan.Ten,
																							     MieuTa = hocPhan.MieuTa,
																							     SoBuoi = hocPhan.SoBuoi,
																							     IdMonHoc = hocPhan.IdMonHoc
																						     };

		public Guid Id { get; set; }
		public string Ten { get; set; } = string.Empty;
		public string MieuTa { get; set; } = string.Empty;
		public Guid IdMonHoc { get; set; }
		public int SoBuoi { get; set; }
	}

	public class Post : IHocPhan
	{
		public string Ten { get; set; } = string.Empty;
		public string MieuTa { get; set; } = string.Empty;
		public Guid IdMonHoc { get; set; }
		public int SoBuoi { get; set; }
	}
}