namespace Api.Models.Interfaces;

interface IMon : IMetadata, IMonInfo, IMonRef, IMonCollection
{
    /// <summary>
    /// Thời gian tạo
    /// </summary>
    /// <value></value>
    DateTime ThoiGianTao { get; set; }
}

public interface IMonInfo
{
    /// <summary>
    /// Tên môn học
    /// </summary>
    /// <value></value>
    string Ten { get; set; }

    /// <summary>
    /// Miêu tả
    /// </summary>
    /// <value></value>
    string? MieuTa { get; set; }
}

public interface IMonRef
{
    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    ICollection<DanhMucMon>? DanhMucMon { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    ICollection<KhoaHoc>? KhoaHoc { get; set; }
}

public interface IMonCollection
{
    /// <summary>
    /// 
    /// </summary>
    /// <value></value>
    public Nguoi? NguoiTao { get; set; }
}