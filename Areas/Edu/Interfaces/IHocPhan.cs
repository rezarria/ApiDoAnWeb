namespace Api.Areas.Edu.Interfaces;

public interface IHocPhan
{
	public string Ten { get; set; }
	public string MieuTa { get; set; }
	public Guid IdMonHoc { get; set; }
	public int SoBuoi { get; set; }
}