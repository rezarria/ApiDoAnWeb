namespace Api.Models;

/// <summary>
/// </summary>
public class HocVien : Nguoi
{
    public string? Truong { get; set; }
    public string? PhuHuynh { get; set; }

    public virtual ICollection<LopHoc>? Lop { get; set; } = null!;
}