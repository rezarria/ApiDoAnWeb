namespace Api.Areas.Edu.Interfaces.SoYeuLyLich;

public interface IQuaTrinhCongTac
{
	public DateTime? ThoiGianBatDau { get; set; }
	public DateTime? ThoiGianKetThuc { get; set; }
	public string? DonViCongTac { get; set; }
	public string? ChucVu { get; set; }
}