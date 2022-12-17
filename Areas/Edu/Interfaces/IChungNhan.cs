namespace Api.Areas.Edu.Interfaces;

public interface IChungNhan
{
	public string Ten { get; set; }
	public string NoiDung { get; set; }
	public Guid? IdMonHoc { get; set; }
	public DateTime ThoiGianTao { get; set; }
}