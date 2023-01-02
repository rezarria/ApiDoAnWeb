#region

using Api.Areas.Edu.Interfaces;
using Microsoft.Build.Framework;
using System.Linq.Expressions;

#endregion

namespace Api.Areas.Edu.DTOs;

public static class KieuNguoiDung
{
	public class Get : IMetadataKey, IKieuNguoiDung
	{
		public static readonly Expression<Func<Models.KieuNguoiDung, Get>> Expression = kieuNguoiDung =>
																							new Get
																							{
																								Id = kieuNguoiDung.Id,
																								Ten = kieuNguoiDung.Ten,
																								IdTruongThongTinNguoiDung = kieuNguoiDung.DanhSachTruongThongTinNguoiDungThuocKieuNguoiDung
																																		 .Select(x => x.IdTruongThongTinNguoiDung).ToList()
																							};

		public ICollection<Guid> IdTruongThongTinNguoiDung { get; set; } = new List<Guid>();
		[Required]
		public string Ten { get; set; } = string.Empty;
		public Guid Id { get; set; }
	}

	public class Post : IKieuNguoiDung
	{
		public ICollection<Guid> IdTruongThongTinNguoiDung { get; set; } = new List<Guid>();
		public string Ten { get; set; } = string.Empty;

		public Models.KieuNguoiDung Convert()
		{
			return new()
				   {
					   Ten = Ten,
					   DanhSachTruongThongTinNguoiDungThuocKieuNguoiDung = IdTruongThongTinNguoiDung.Select(x =>
																										        new Models.DanhSachTruongThongTinNguoiDungThuocKieuNguoiDung { IdTruongThongTinNguoiDung = x })
																								    .ToList()
				   };
		}
	}
}