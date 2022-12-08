using Api.Areas.Edu.Interfaces;

namespace Api.Areas.Edu.DTOs;

public class HocPhan
{
    public class Get : IHocPhan
    {
        public  Guid Id { get; set; }

        public Models.HocPhan Convert()
            => new()
            {
                Ten = Ten,
                SoBuoi = SoBuoi,
                MieuTa = MieuTa,
                MonHoc = new()
                {
                    Id = IdMon
                }
            };

        public string Ten { get; set; } = string.Empty;
        public string MieuTa { get; set; }  = string.Empty;
        public Guid IdMon { get; set; }
        public int SoBuoi { get; set; }
    }

    public class Post : Get
    {
    }
}