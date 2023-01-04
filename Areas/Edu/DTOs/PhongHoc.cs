#region

using Api.Areas.Edu.Interfaces;
using System.Linq.Expressions;

#endregion

namespace Api.Areas.Edu.DTOs;

public static class PhongHoc
{
	public class Get : IPhongHoc
	{
		public static readonly Expression<Func<Models.PhongHoc, Get>> Expression = phongHoc =>
																					   new Get
																					   {
																						   Id = phongHoc.Id,
																						   Ten = phongHoc.Ten,
																						   ViTri = phongHoc.ViTri
																					   };

		public static readonly Expression<Func<Models.PhongHoc, dynamic>> ExpressionToiThieu = phongHoc =>
																								   new
																								   {
																									   phongHoc.Id,
																									   phongHoc.Ten
																								   };

		public Guid Id { get; set; }
		public string Ten { get; set; } = string.Empty;
		public string ViTri { get; set; } = string.Empty;
	}

	public class Post : IPhongHoc
	{
		public string Ten { get; set; } = string.Empty;
		public string ViTri { get; set; } = string.Empty;
	}
}