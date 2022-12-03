using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public class PhongHoc
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string Ten { get; set; } = string.Empty;

    [Required]
    public virtual CoSoDaoTao? CoSoDaoTao { get; set; }

    public virtual ICollection<Lop>? Lop { get; set; }
    public virtual ICollection<Lich>? Lich { get; set; }

    [Timestamp]
    public byte[]? RowVersion { get; set; } = null!;
}
