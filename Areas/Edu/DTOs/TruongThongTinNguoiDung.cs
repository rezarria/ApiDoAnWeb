#region

using Api.Areas.Edu.Interfaces;
using System.Linq.Expressions;

#endregion

namespace Api.Areas.Edu.DTOs;

public static class TruongThongTinNguoiDung
{
	public class Get : IMetadataKey, ITruongThongTinNguoiDungInfo
	{
		public static readonly Expression<Func<Models.TruongThongTinNguoiDung, Get>> Expression = truongThongTin =>
																									  new Get
																									  {
																										  Id = truongThongTin.Id,
																										  Ten = truongThongTin.Ten,
																										  Alias = truongThongTin.Alias,
																										  KieuDuLieu = truongThongTin.KieuDuLieu
																									  };

		public Guid Id { get; set; }
		public string? Ten { get; set; }
		public string? Alias { get; set; }
		public string KieuDuLieu { get; set; } = string.Empty;
	}

	public class Post : ITruongThongTinNguoiDungInfo
	{
		public string? Ten { get; set; }
		public string? Alias { get; set; }
		public string KieuDuLieu { get; set; } = string.Empty;
	}
}