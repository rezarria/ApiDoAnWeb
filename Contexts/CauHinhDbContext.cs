using Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Contexts;

public class CauHinhDbContext : DbContext
{
	public CauHinhDbContext(DbContextOptions<CauHinhDbContext> options) : base(options)
	{
	}

	public DbSet<Client> Client { get; set; } = null!;

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		modelBuilder.Entity<Client>(entity =>
								    {
									    entity.HasIndex(x => x.Url).IsUnique();
								    });
	}
}