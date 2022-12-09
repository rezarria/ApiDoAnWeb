namespace Api.Areas.Edu.Interfaces;

public interface ILich
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