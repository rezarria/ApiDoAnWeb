using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public class PhongHoc
{
    [Key]
    public Guid Id { get; set; }

    [Required(AllowEmptyStrings = false)]
    public string Ten { get; set; } = string.Empty;

    public string ViTri { get; set; } = string.Empty;

    public virtual CoSoDaoTao? CoSoDaoTao { get; set; }

    public virtual ICollection<LopHoc>? Lop { get; set; }
    public virtual ICollection<Lich>? Lich { get; set; }

    [Timestamp]
    public byte[]? RowVersion { get; set; } = null!;
}
