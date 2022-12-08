using Microsoft.AspNetCore.Mvc;

namespace Api.Areas.Api.Controllers;

public partial class HocPhan
{
    public static class DTO
    {
        public class Post
        {
            public string Ten { get; set; } = string.Empty;
            public string MieuTa { get; set; } = string.Empty;
            public int SoBuoi { get; set; }
            public Guid IdMonHoc { get; set; }
            public Models.HocPhan Convert()
                => new()
                {
                    Ten = Ten,
                    SoBuoi = SoBuoi,
                    MieuTa = MieuTa,
                    Mon = new()
                    {
                        Id = IdMonHoc
                    }
                };
        }
    }
}