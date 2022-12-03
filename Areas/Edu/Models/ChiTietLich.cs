using System.ComponentModel.DataAnnotations;

namespace Api.Models;

public class ChiTietLich
{
    [Key] public Guid Id { get; set; }

    [Required] public virtual Lich Lich { get; set; } = null!;

    [Required]
    public virtual Nguoi Nguoi { get; set; } = null!;

    public virtual DiemDanh? DiemDanh { get; set; }

    public virtual Nguoi? NguoiThaoTac { get; set; }

    [Timestamp] public byte[]? RowVersion { get; set; } = null!;
}