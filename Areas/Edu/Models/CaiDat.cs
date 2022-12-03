using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public class CaiDat
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public String Key { get; set; } = null!;
    public String? Value { get; set; }
    public DateTime ThoiGianCapNhat { get; set; }

    [Timestamp]
    public byte[]? RowVersion { get; set; } = null!;
}