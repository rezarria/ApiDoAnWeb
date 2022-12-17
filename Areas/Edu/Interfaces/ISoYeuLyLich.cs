namespace Api.Areas.Edu.Interfaces;

public enum GioiTinh
{
	Nam,
	Nu
}

public interface ISoYeuLyLichModel : IMetadataConcurrency, ISoYeuLyLich
{
}

public interface ISoYeuLyLich : IMetadataKey, ISoYeuLyLichInfo
{
}

public interface ISoYeuLyLichInfo
{
	public string? HoVaTen { get; set; }
	public GioiTinh? GioiTinh { get; set; }
	public DateTime? NgaySinh { get; set; }
	public string? NoiSinh { get; set; }
	public string? NguyenQuan { get; set; }
	public string? ThuongTru { get; set; }
	public string? SoDienThoai { get; set; }
	public string? Email { get; set; }
	public string? DanToc { get; set; }
	public string? TonGiao { get; set; }
	public string? TrinhDoVanHoa { get; set; }
	public string? SoTruong { get; set; }
}