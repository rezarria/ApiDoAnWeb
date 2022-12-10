using System.Linq.Expressions;
using Api.Areas.Edu.Interfaces;

namespace Api.Areas.Edu.DTOs;

public class HocPhan
{
    public class Get : IHocPhan
    {
        public Guid Id { get; set; }
        public string Ten { get; set; } = string.Empty;
        public string MieuTa { get; set; } = string.Empty;
        public Guid IdMon { get; set; }
        public int SoBuoi { get; set; }

        public static readonly Expression<Func<Models.HocPhan, Get>> Expression = hocPhan => new()
        {
            Id = hocPhan.Id,
            Ten = hocPhan.Ten,
            MieuTa = hocPhan.MieuTa,
            SoBuoi = hocPhan.SoBuoi,
            IdMon = hocPhan.IdMon
        };
    }

    public class Post : IHocPhan
    {
        public string Ten { get; set; } = string.Empty;
        public string MieuTa { get; set; } = string.Empty;
        public Guid IdMon { get; set; }
        public int SoBuoi { get; set; }
    }
}