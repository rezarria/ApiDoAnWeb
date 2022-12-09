namespace Api.Areas.Edu.Interfaces;

public interface INguoiDung
{
	Guid? SoYeuLyLichId { get; set; }


	string PhanLoai => GetType().Name;
}