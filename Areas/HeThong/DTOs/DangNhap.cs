namespace Api.Areas.HeThong.DTOs;

public static class DangNhap
{
	public class YeuCauDangNhap
	{
		public Guid? Id { get; set; }
		public string? UserName { get; set; }
		public string? Password { get; set; }
	}
}