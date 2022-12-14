namespace Api.Areas.Edu.Interfaces;

public interface IDiemDanh
{
	public enum TrangThaiDiemDanh
	{
		Vang,
		CoMat,
		CoPhep,
		Muon
	}

	public TrangThaiDiemDanh TrangThai { get; set; }
	public string? NhanXet { get; set; }
	public DateTime ThoiDiemDiemDanh { get; set; }
	public Guid? IdChiTietLich { get; set; }
}