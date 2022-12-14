using System.Linq.Expressions;
using Api.Areas.Edu.Interfaces;
using Microsoft.Build.Framework;

namespace Api.Areas.Edu.DTOs;

public static class GiaTriTruongThongTinNguoiDung
{
	public class Get : IMetadataKey, IGiaTriTruongThongTinNguoiDung
	{
		public Guid Id { get; set; }
		public Guid IdNguoiDung { get; set; }
		public Guid IdTruongThongTinNguoiDung { get; set; }
		public string GiaTri { get; set; } = string.Empty;

		public static readonly Expression<Func<Models.GiaTriTruongThongTinNguoiDung, Get>> Expression = truongGiaTri =>
			new()
			{
				Id = truongGiaTri.Id,
				IdNguoiDung = truongGiaTri.IdNguoiDung,
				IdTruongThongTinNguoiDung = truongGiaTri.IdTruongThongTinNguoiDung,
				GiaTri = truongGiaTri.GiaTri
			};
	}

	public class Post : IGiaTriTruongThongTinNguoiDung
	{
		[Required]
		public Guid IdNguoiDung { get; set; }
		[Required]
		public Guid IdTruongThongTinNguoiDung { get; set; }
		[Required]
		public string GiaTri { get; set; } = string.Empty;

	}
}