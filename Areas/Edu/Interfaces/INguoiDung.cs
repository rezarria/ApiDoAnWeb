namespace Api.Areas.Edu.Interfaces;

public interface INguoiDung
{
	Guid? IdSoYeuLyLich { get; set; }


	string PhanLoai => GetType().Name;
}