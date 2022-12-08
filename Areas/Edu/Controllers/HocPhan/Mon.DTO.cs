using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Areas.Api.Controllers;

public partial class MonController : ControllerBase
{
    public static class DTO
    {
        public static Expression<Func<Models.Mon, dynamic>> expressionToiThieu = (Models.Mon mon) => new
        {
            mon.Id,
            mon.Ten
        };

        public class Get
        {
            public Guid Id { get; set; }
            public string Ten { get; set; } = string.Empty;
            public string MieuTa { get; set; } = string.Empty;

            public Models.Mon Convert()
                => new()
                {
                    Id = Id,
                    Ten = Ten,
                    MieuTa = MieuTa
                };

            public static Expression<Func<Models.Mon, Get>> expression = mon => new()
            {
                Id = mon.Id,
                Ten = mon.Ten,
                MieuTa = mon.MieuTa
            };
        }
    }
}