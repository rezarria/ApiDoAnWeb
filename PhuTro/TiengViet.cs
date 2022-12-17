namespace Api.PhuTro;

public static class TiengViet
{
	private static readonly string[] bangChu =
	{
		"aAeEoOuUiIdDyY",
		"áàạảãâấầậẩẫăắằặẳẵ",
		"ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
		"éèẹẻẽêếềệểễ",
		"ÉÈẸẺẼÊẾỀỆỂỄ",
		"óòọỏõôốồộổỗơớờợởỡ",
		"ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
		"úùụủũưứừựửữ",
		"ÚÙỤỦŨƯỨỪỰỬỮ",
		"íìịỉĩ",
		"ÍÌỊỈĨ",
		"đ",
		"Đ",
		"ýỳỵỷỹ",
		"ÝỲỴỶỸ"
	};

	public static string LoaiBoDau(this string xau)
	{
		for (var i = 1; i < bangChu.Length; i++)
		for (var j = 0; j < bangChu[i].Length; j++)
			xau = xau.Replace(bangChu[i][j], bangChu[0][i - 1]);
		return xau;
	}
}