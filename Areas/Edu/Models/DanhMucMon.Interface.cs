namespace Api.Models.Interfaces;

public interface IDanhMucMon : IMetadata, IDanhMucMonInfo, IDanhMucMonCollection, IDanhMucMOnRef
{
}

public interface IDanhMucMonInfo
{
    /// <summary>
    /// </summary>
    string Ten { get; set; }

    /// <summary>
    /// </summary>
    string? MieuTa { get; set; }

    /// <summary>
    /// 
    /// </summary>
    DateTime ThoiGianTao { get; set; }

}

public interface IDanhMucMonCollection
{
    /// <summary>
    /// 
    /// </summary>
    ICollection<Mon>? Mon { get; set; }
}

public interface IDanhMucMOnRef
{
    /// <summary>
    /// </summary>

    Nguoi? NguoiTao { get; set; }
}