using System.Linq.Expressions;

namespace Api.Areas.Edu.DTOs;

public class LopHoc
{
    public class Get
    {
        public Guid Id { get; set; }
        public string Ten { get; set; } = string.Empty;
        public int SoBuoi { get; set; }
        public DateTime ThoiGianBatDau { get; set; }
        public DateTime ThoiGianKetThuc { get; set; }
        public HocPhan.Get? HocPhan { get; set; }

        public static Expression<Func<Models.LopHoc, Get>> Expression = lop => new Get()
        {
            Id = lop.Id,
            Ten = lop.Ten,
            SoBuoi = lop.SoBuoi,
            ThoiGianBatDau = lop.ThoiGianBatDau,
            ThoiGianKetThuc = lop.ThoiGianKetThuc,
            HocPhan = new()
            {
                
            }
        };
    }

    public class Post : Get
    {
        public Models.LopHoc Convert()
        {
            Models.LopHoc data = new()
            {
                Id = Id,
                SoBuoi = SoBuoi,
                ThoiGianBatDau = ThoiGianBatDau,
                ThoiGianKetThuc = ThoiGianKetThuc,
            };
            if (HocPhan is not null)
                data.HocPhan = new()
                {
                    Id = HocPhan.Id
                };
            return data;
        }
    }
}