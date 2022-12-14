using System.Linq.Expressions;
using Api.Areas.Edu.Interfaces;

namespace Api.Areas.Edu.DTOs;

public static class TruongThongTinNguoiDung
{
    public class Get : IMetadataKey, ITruongThongTinNguoiDungInfo
    {
        public Guid Id { get; set; }
        public string? Ten { get; set; } = string.Empty;
        public string KieuDuLieu { get; set; } = string.Empty;

        public static readonly Expression<Func<Models.TruongThongTinNguoiDung, Get>> Expression = truongThongTin =>
            new()
            {
                Id = truongThongTin.Id,
                Ten = truongThongTin.Ten,
                KieuDuLieu = truongThongTin.KieuDuLieu
            };
    }

    public class Post : ITruongThongTinNguoiDungInfo
    {
        public string? Ten { get; set; } = string.Empty;
        public string KieuDuLieu { get; set; } = string.Empty;
    }
}