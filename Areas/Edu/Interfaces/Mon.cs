namespace Api.Areas.Edu.Interfaces;

public interface IMonHoc
{
	public string Ten { get; set; }
	public  int SoBuoi { get; set; }
	public string? MieuTa { get; set; }
	public DateTime ThoiGianTao { get; set; }
	public DateTime ThoiGianBatDau { get; set; }
	public  DateTime ThoiGianKetThuc { get; set; }
}