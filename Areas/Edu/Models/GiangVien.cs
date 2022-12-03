using System.ComponentModel.DataAnnotations;

namespace Api.Models;

/// <summary>
/// </summary>
public class GiangVien : Nguoi
{
    [Required(ErrorMessage = "Vui lòng chọn trình độ")]
    public string? TrinhDo { get; set; }

    [Required(ErrorMessage = "Vui lòng điền đơn vị công tác")]
    public string? DonViCongTac { get; set; }

    /// <summary>
    /// </summary>
    /// <value></value>
    public virtual ICollection<LopHoc>? Lop { get; set; } = null!;
}