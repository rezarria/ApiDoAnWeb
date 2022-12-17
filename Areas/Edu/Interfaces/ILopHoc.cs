namespace Api.Areas.Edu.Interfaces;

public interface ILopHoc
{
	public string Ten { get; set; }
	public int SoBuoi { get; set; }
	public DateTime ThoiGianBatDau { get; set; }
	public DateTime ThoiGianKetThuc { get; set; }
	public TrangThaiLopHoc TrangThai { get; set; }
	public Guid IdHocPhan { get; set; }

	public enum TrangThaiLopHoc
	{
		Chua,
		Dang,
		Xong
	}
}