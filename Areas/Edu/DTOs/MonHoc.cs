#region

using Api.Areas.Edu.Interfaces;
using System.Linq.Expressions;

#endregion

namespace Api.Areas.Edu.DTOs;

public static class MonHoc
{
	public class Get : IMonHoc
	{
		public static Expression<Func<Models.MonHoc, Get>> Expression = monHoc => new Get(monHoc.Id, monHoc.Ten, monHoc.MieuTa);

		public static Expression<Func<Models.MonHoc, dynamic>> ExpressionToiThieu = monHoc => new
																							  {
																								  monHoc.Id,
																								  monHoc.Ten
																							  };

		public Get()
		{
		}

		public Get(Guid id, string? ten, string? mieuTa)
		{
			Id = id;
			if (ten is not null)
				Ten = ten;
			if (mieuTa is not null)
				MieuTa = mieuTa;
		}

		public Guid Id { get; set; }
		public string Ten { get; set; } = string.Empty;
		public string? MieuTa { get; set; }
	}

	public class Post : IMonHoc
	{
		public string Ten { get; set; } = string.Empty;
		public string? MieuTa { get; set; }
	}
}