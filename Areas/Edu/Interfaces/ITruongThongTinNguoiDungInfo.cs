namespace Api.Areas.Edu.Interfaces;

public interface ITruongThongTinNguoiDungInfoModel : IMetadata, ITruongThongTinNguoiDungInfo
{
}

public interface ITruongThongTinNguoiDungInfo
{
	public string TieuDe { get; set; }
	public string KieuDuLieu { get; set; }
}