#region

using System.Linq.Expressions;
using Api.Areas.Edu.Interfaces;
using Microsoft.Build.Framework;

#endregion

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

	public class GetPhuTro
	{
		public Guid Id { get; set; }
		public Guid IdTruongThongTinNguoiDung { get; set; }
		public string GiaTri { get; set; } = string.Empty;

		public static readonly Expression<Func<Models.GiaTriTruongThongTinNguoiDung, GetPhuTro>> Expression =
			truongGiaTri => new()
			{
				Id = truongGiaTri.Id,
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

	public class Patch
	{
		public Guid? Id { get; set; }
		public Guid? IdTruongThongTinNguoiDung { get; set; }
		[Required]
		public string GiaTri { get; set; } = string.Empty;

		public static Models.GiaTriTruongThongTinNguoiDung Convert(Patch obj)
		{
			Models.GiaTriTruongThongTinNguoiDung model = new()
			{
				GiaTri = obj.GiaTri
			};
			if (obj.Id is not null) model.Id = obj.Id.Value;
			if (obj.IdTruongThongTinNguoiDung is not null) model.IdTruongThongTinNguoiDung = obj.IdTruongThongTinNguoiDung.Value;
			return model;
		}
	}
}