namespace Api.Models.Interfaces;

public interface INguoiDung : IMetadata, INguoiDungInfo, INguoiRef, INguoiCollection
{
}

public interface INguoiDungInfo
{
    Guid? SoYeuLyLichId { get; set; }

    /// <summary>
    /// </summary>
    /// <value></value>
    SoYeuLyLich.SoYeuLyLich? SoYeuLyLich { get; set; }

    string PhanLoai => GetType().Name;
}

public interface INguoiRef
{
    ITaiKhoan? TaiKhoan { get; set; }
}

public interface INguoiCollection
{
    ICollection<ChiTietLich>? ChiTietLich { get; set; }
}