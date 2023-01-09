using Api.Areas.Edu.Contexts;
using Api.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Api.Services;

public class QuanLyTepTin
{
	private readonly ElFinderDbContext _elFinderDbContext;
	private readonly AppDbContext _appDbContext;

	public QuanLyTepTin(ElFinderDbContext elFinderDbContext, AppDbContext appDbContext)
	{
		_elFinderDbContext = elFinderDbContext;
		_appDbContext = appDbContext;
	}

	public async Task DongDoTaiKhoan(CancellationToken cancellationToken = default)
	{
		await _appDbContext.TaiKhoan.ForEachAsync(x =>
												  {
													  if (!_elFinderDbContext.User.Any(y => y.Id == x.Id))
													  {
														  _elFinderDbContext.User.Add(new()
																				      {
																					      Id = x.Id,
																					      VolumePath = x.Id + x.Username
																				      });
													  }
												  }, cancellationToken);
		await _elFinderDbContext.SaveChangesAsync(cancellationToken);
	}
}