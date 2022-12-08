namespace Api.Areas.Edu.Interfaces;

public interface ILich : ILichInfo, ILichNavigation, ILichCollection
{
}

public interface ILichInfo
{
    public enum TinhTrang
    {
        ChuaBatDau,
        DangTiepDien,
        DaXong,
        DaHuy
    }

    public DateTime ThoiGianBatDau { get; set; }
    public DateTime ThoiGianKetThuc { get; set; }
    public string? MoTa { get; set; }
    public TinhTrang TinhTrangLich { get; set; }
}

public interface ILichNavigation
{
    public abstract ILopHoc? Lop { get; set; }
    public abstract IPhongHoc? PhongHoc { get; set; }
}

public interface ILichCollection
{
    public abstract ICollection<IChiTietLich>? ChiTietLich { get; set; }
}