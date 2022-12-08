namespace Api.Areas.Edu.Interfaces;

public interface IDanhMucMon : IDanhMucMonInfo, IDanhMucMonCollection, IDanhMucMonNavigation
{
}

public interface IDanhMucMonInfo
{
	string Ten { get; set; }
	string? MieuTa { get; set; }
	DateTime ThoiGianTao { get; set; }
}

public interface IDanhMucMonCollection
{
	ICollection<IMonHoc>? Mon { get; set; }
}

public interface IDanhMucMonNavigation
{
	INguoiDung? NguoiTao { get; set; }
}