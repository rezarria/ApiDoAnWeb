using System.Linq.Expressions;
using Api.Areas.Edu.Interfaces;

namespace Api.Areas.Edu.DTOs;

public static class PhongHoc
{
    public class Get : IPhongHoc
    {
        public Guid Id { get; set; }
        public string Ten { get; set; } = string.Empty;
        public string ViTri { get; set; } = string.Empty;

        public static Expression<Func<Models.PhongHoc, Get>> Expression = phongHoc =>
            new()
            {
                Id = phongHoc.Id,
                Ten = phongHoc.Ten,
                ViTri = phongHoc.ViTri
            };

        public static Expression<Func<Models.PhongHoc, dynamic>> ExpressionToiThieu = phongHoc =>
            new
            {
                phongHoc.Id,
                phongHoc.Ten
            };
    }

    public class Post : IPhongHoc
    {
        public string Ten { get; set; } = string.Empty;
        public string ViTri { get; set; } = string.Empty;
    }
}