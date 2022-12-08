using System.ComponentModel.DataAnnotations;
using Api.Areas.Edu.Interfaces;

namespace Api.Areas.Edu.Models;

public class ChiTietLich : IChiTietLich
{
    [Key] public Guid Id { get; set; }

    public Guid IdLich { get; set; }

    [Required] public virtual Lich Lich { get; set; } = null!;

    [Required] public virtual NguoiDung NguoiDung { get; set; } = null!;

    public virtual DiemDanh? DiemDanh { get; set; }

    public virtual NguoiDung? NguoiThaoTac { get; set; }

    [Timestamp] public byte[]? RowVersion { get; set; } = null!;
}