#region

using Api.Areas.Edu.Interfaces;
using Microsoft.Build.Framework;
using System.Linq.Expressions;

#endregion

namespace Api.Areas.Edu.DTOs;

public static class NguoiDung
{
	public class Get : IMetadataKey
	{
		public static readonly Expression<Func<Models.NguoiDung, Get>> Expression = nguoiDung =>
																						new Get(nguoiDung.Id, nguoiDung.IdKieuNguoiDung, nguoiDung.SoYeuLyLich, nguoiDung.GiaTriTruongThongTinNguoiDung);

		public Get(Guid id, Guid? idKieuNguoiDung, ISoYeuLyLich? soYeuLyLich, ICollection<Models.GiaTriTruongThongTinNguoiDung>? giaTriTruongThongTinNguoiDung)
		{
			Id = id;
			IdKieuNguoiDung = idKieuNguoiDung;
			if (soYeuLyLich is not null)
				SoYeuLyLich = soYeuLyLich.Convert<ISoYeuLyLich, SoYeuLyLich.Get>();

			if (giaTriTruongThongTinNguoiDung is not null)
				GiaTriTruongThongTinNguoiDung = giaTriTruongThongTinNguoiDung
												.Select(x => new GiaTriTruongThongTinNguoiDungDto
														     {
															     Id = x.IdTruongThongTinNguoiDung,
															     GiaTri = x.GiaTri
														     }).ToList();
		}

		public Guid? IdKieuNguoiDung { get; set; }

		public SoYeuLyLich.Get? SoYeuLyLich { get; }

		public ICollection<GiaTriTruongThongTinNguoiDungDto>? GiaTriTruongThongTinNguoiDung { get; }

		public Guid Id { get; set; }

		public class GiaTriTruongThongTinNguoiDungDto
		{
			public Guid Id { get; set; }
			public string GiaTri { get; set; } = string.Empty;
		}
	}

	public class Post
	{
		[Required]
		public Guid IdKieuNguoiDung { get; set; }

		public SoYeuLyLich.Post SoYeuLyLich { get; set; } = null!;

		public ICollection<TruongGiaTriDto>? TruongGiaTri { get; set; }

		public Models.NguoiDung Convert()
		{
			Models.NguoiDung data = new()
									{
										IdKieuNguoiDung = IdKieuNguoiDung,
										SoYeuLyLich = SoYeuLyLich.Convert<SoYeuLyLich.Post, Models.SoYeuLyLich>()
									};

			if (TruongGiaTri is not null)
				data.GiaTriTruongThongTinNguoiDung = TruongGiaTri.Select(x => new Models.GiaTriTruongThongTinNguoiDung
																			  {
																				  IdTruongThongTinNguoiDung = x.Id,
																				  GiaTri = x.GiaTri
																			  }).ToList();

			return data;
		}

		public class TruongGiaTriDto
		{
			[Required]
			public Guid Id { get; set; }


			[Required]
			public string GiaTri { get; set; } = string.Empty;
		}
	}
}