namespace Api.Areas.Edu.Interfaces;
public enum GioiTinh
{
	Nam,
	Nu
}

public interface ISoYeuLyLich
{
	public string? HoVaTen { get; set; }
	public GioiTinh? GioiTinh { get; set; }
	public DateTime? SinhNgay { get; set; }
	public string? NoiSinh { get; set; }
	public string? NguyenQuan { get; set; }
	public string? NoiDangKyHoKhauThuongTru { get; set; }
	public string? DienThoai { get; set; }
	public string? Email { get; set; }
	public string? DanToc { get; set; }
	public string? TonGiao { get; set; }
	public string? TrinhDoVanHoa { get; set; }
	public DateTime? TNCS_NgayKetNap { get; set; }
	public string? TNCS_NoiKetNap { get; set; }
	public DateTime? CSVN_NgayKetNap { get; set; }
	public string? CSVN_NoiKetNap { get; set; }
	public string? KhenThuong_KyLuat { get; set; }
	public string? SoTruong { get; set; }
}