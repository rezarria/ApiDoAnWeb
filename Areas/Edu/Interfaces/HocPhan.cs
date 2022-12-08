namespace Api.Areas.Edu.Interfaces;

public interface IHocPhan
{
	public string Ten { get; set; }
	public string MieuTa { get; set; }
	public Guid IdMon { get; set; }
	public int SoBuoi { get; set; }
}