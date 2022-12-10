using System.Linq.Expressions;
using Api.Areas.Edu.Interfaces;

namespace Api.Areas.Edu.DTOs;

public static class NguoiDung
{
    public class Get
    {
        public Get(ISoYeuLyLich? soYeuLyLich)
        {
            if (soYeuLyLich is not null)
                this.SoYeuLyLich = soYeuLyLich.Convert<ISoYeuLyLich, DTOs.SoYeuLyLich.Get>();
        }

        public SoYeuLyLich.Get? SoYeuLyLich { get; set; }

        public static readonly Expression<Func<Models.NguoiDung, Get>> Expression = nguoiDung =>
            new(nguoiDung.SoYeuLyLich);
    }

    public class Post
    {
        public DTOs.SoYeuLyLich.Post SoYeuLyLich { get; set; } = null!;

        public Models.NguoiDung Convert() => new()
        {
            SoYeuLyLich = SoYeuLyLich.Convert<DTOs.SoYeuLyLich.Post, Models.SoYeuLyLich>()
        };
    }
}