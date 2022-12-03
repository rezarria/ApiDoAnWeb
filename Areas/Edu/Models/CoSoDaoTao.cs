using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public class CoSoDaoTao
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string Ten { get; set; } = string.Empty;

    public string DiaChi { get; set; } = string.Empty;

    public virtual ICollection<PhongHoc>? PhongHoc { get; set; }

    [Timestamp]
    public byte[]? RowVersion { get; set; } = null!;
}
