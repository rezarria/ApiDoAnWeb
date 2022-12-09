namespace Api.Areas.Edu.Interfaces.SoYeuLyLich;

public interface IQuaTrinhDaoTao
{
	public DateTime? ThoiGianBatDau { get; set; }
	public DateTime? ThoiGianKetThuc { get; set; }
	public string? CoSoDaoTao { get; set; }
	public string? HinhThucDaoTao { get; set; }
	public string? VanBangChungChi { get; set; }
}