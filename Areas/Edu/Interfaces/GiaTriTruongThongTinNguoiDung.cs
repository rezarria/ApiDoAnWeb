namespace Api.Areas.Edu.Interfaces;

public interface IGiaTriTruongThongTinNguoiDungModel : IMetadata, IGiaTriTruongThongTinNguoiDung
{
}

public interface IGiaTriTruongThongTinNguoiDung
{
	public Guid IdNguoiDung { get; set; }
	public Guid IdTruongThongTinNguoiDung { get; set; }

	public string GiaTri { get; set; }
}