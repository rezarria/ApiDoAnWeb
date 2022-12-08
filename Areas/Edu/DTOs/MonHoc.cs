using System.Linq.Expressions;
using Api.Areas.Edu.Interfaces;

namespace Api.Areas.Edu.DTOs;

public static class MonHoc
{
    public  class Get : IMonHoc
    {
        public  Guid Id { get; set; }
        public string Ten { get; set; }
        public int SoBuoi { get; set; }
        public string? MieuTa { get; set; }
        public DateTime ThoiGianTao { get; set; }
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