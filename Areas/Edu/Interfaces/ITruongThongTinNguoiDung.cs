namespace Api.Areas.Edu.Interfaces;

public interface ITruongThongTinNguoiDungInfoModel : IMetadata, ITruongThongTinNguoiDungInfo
{
}

public interface ITruongThongTinNguoiDungInfo
{
	public string? Ten { get; set; }
	public string? Alias { get; set; }
	public string KieuDuLieu { get; set; }
}