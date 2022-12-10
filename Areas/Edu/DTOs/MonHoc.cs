using System.Linq.Expressions;
using Api.Areas.Edu.Interfaces;

namespace Api.Areas.Edu.DTOs;

public static class MonHoc
{
    public class Get : IMonHoc
    {
        public Get()
        {
        }

        public Get(Guid id, string? mieuTa)
        {
            Id = id;
            MieuTa = mieuTa;
        }

        public Guid Id { get; set; }
        public string Ten { get; set; } = string.Empty;
        public string? MieuTa { get; set; }

        public static Expression<Func<Models.MonHoc, Get>> Expression = monHoc => new(monHoc.Id, monHoc.Ten);

        public static Expression<Func<Models.MonHoc, dynamic>> ExpressionToiThieu = monHoc => new
        {
            monHoc.Id,
            monHoc.Ten
        };
    }

    public class Post : IMonHoc
    {
        public string Ten { get; set; } = string.Empty;
        public string? MieuTa { get; set; }
    }
}