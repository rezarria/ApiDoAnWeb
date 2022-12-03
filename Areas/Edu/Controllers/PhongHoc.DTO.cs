using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Areas.Edu.Controllers;

public partial class PhongHoc : ControllerBase
{
	Expression<Func<Models.PhongHoc, DTO.Get>> expressionGet = x => new(
		x.Id, x.Ten, x.ViTri
	)
	;

	public static class DTO
	{
		public class Get
		{
			public Get(Guid id, string ten, string viTri)
			{
				Id = id;
				Ten = ten;
				ViTri = viTri;
			}

			public Guid Id { get; set; }
			public string Ten { get; set; } = string.Empty;
			public string ViTri { get; set; } = string.Empty;
		}


		public class Post
		{
			public Post(string ten, string viTri)
			{
				Ten = ten;
				ViTri = viTri;
			}

			public string Ten { get; set; } = string.Empty;
			public string ViTri { get; set; } = string.Empty;

			public Models.PhongHoc Convert() => new()
			{
				Ten = Ten,
				ViTri = ViTri
			};
		}
	}
}