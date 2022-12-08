using System.ComponentModel.DataAnnotations;
using Api.Areas.Edu.Interfaces;

namespace Api.Areas.Edu.Models;

public class PhongHoc : IMetadata, IPhongHoc
{
    [Key]
    public Guid Id { get; set; }

    [Required(AllowEmptyStrings = false)]
    public string Ten { get; set; } = string.Empty;

    public string ViTri { get; set; } = string.Empty;

    public virtual ICollection<LopHoc>? Lop { get; set; }
    public virtual ICollection<Lich>? Lich { get; set; }

    [Timestamp]
    public byte[]? RowVersion { get; set; } = null!;
}
