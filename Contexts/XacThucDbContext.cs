using Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Api.Contexts;

public class XacThucContext : DbContext
{
	public XacThucContext(DbContextOptions<XacThucContext> options) : base(options)
	{
	}

	public DbSet<TokenDangXuat> TokenDangXuat { get; set; } = null!;

	protected override void OnModelCreating(ModelBuilder builder)
	{
		base.OnModelCreating(builder);
		builder.Entity<TokenDangXuat>(entity =>
									  {
										  entity.Property(x => x.Token).ValueGeneratedNever();
										  entity.Property(x => x.Exp).HasConversion(new TimeSpanToTicksConverter());
									  });
	}
}