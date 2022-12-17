namespace Api.Areas.Edu.Interfaces;

public interface IKieuNguoiDungModel : IMetadata, IKieuNguoiDung
{
}

public interface IKieuNguoiDung
{
	public string Ten { get; set; }
}