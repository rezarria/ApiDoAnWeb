namespace Api.Areas.Edu.Interfaces;

public interface ICoSoDaoTao : ICoSoDaoTaoInfo, ICoSoDaoTaoCollection
{
}

public interface ICoSoDaoTaoInfo
{
	public string Ten { get; set; }
	public string DiaChi { get; set; }
}

public interface ICoSoDaoTaoCollection
{
	public ICollection<IPhongHoc>? PhongHoc { get; set; }
}