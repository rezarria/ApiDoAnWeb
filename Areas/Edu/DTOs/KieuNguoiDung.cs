using System.Linq.Expressions;
using Api.Areas.Edu.Interfaces;
using Microsoft.Build.Framework;

namespace Api.Areas.Edu.DTOs;

public static class KieuNguoiDung
{
	public class Get : IMetadataKey, IKieuNguoiDung
	{
		public Guid Id { get; set; }
		[Required]
		public string Ten { get; set; } = string.Empty;

		public ICollection<Guid> IdTruongThongTinNguoiDung { get; set; } = new List<Guid>();

		public static readonly Expression<Func<Models.KieuNguoiDung, Get>> Expression = kieuNguoiDung =>
			new()
			{
				Id = kieuNguoiDung.Id,
				Ten = kieuNguoiDung.Ten,
				IdTruongThongTinNguoiDung = kieuNguoiDung.TruongThongTin.Select(x => x.Id).ToList()
			};

	}

	public class Post : IKieuNguoiDung
	{
		public string Ten { get; set; } = string.Empty;
		public ICollection<Guid> IdTruongThongTinNguoiDung { get; set; } = new List<Guid>();

		public Models.KieuNguoiDung Convert()
		=> new()
		{
			Ten = Ten,
			TruongThongTin = IdTruongThongTinNguoiDung.Select(x => new Models.TruongThongTinNguoiDung() { Id = x }).ToList()
		};
	}
}