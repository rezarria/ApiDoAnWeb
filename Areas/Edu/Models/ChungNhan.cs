using System.ComponentModel.DataAnnotations;

namespace Api.Areas.Edu.Models;

public class ChungNhan
{
    [Key]
    public Guid Id { get; set; }
    public string Ten { get; set; } = string.Empty;

    public string NoiDung { get; set; } = string.Empty;

    public virtual MonHoc? Mon { get; set; }
    public virtual ICollection<HocPhan>? HocPhan { get; set; }

    public DateTime ThoiGianTao { get; set; }

    [Timestamp]
    public byte[]? RowVersion { get; set; } = null!;
}