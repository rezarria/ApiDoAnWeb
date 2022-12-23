#region

using System.Linq.Expressions;
using Api.Areas.Edu.Interfaces;

#endregion

namespace Api.Areas.Edu.DTOs;

public static class MonHoc
{
	public class Get : IMonHoc
	{
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

		public static Expression<Func<Models.MonHoc, Get>> Expression = monHoc => new(monHoc.Id, monHoc.Ten, monHoc.MieuTa);

		public static Expression<Func<Models.MonHoc, dynamic>> ExpressionToiThieu = monHoc => new
		{
			monHoc.Id,
			monHoc.Ten
		};
	}

	public class Post : IMonHoc
	{
		public string Ten { get; set; } = string.Empty;
		public string? MieuTa { get; set; }
	}
}