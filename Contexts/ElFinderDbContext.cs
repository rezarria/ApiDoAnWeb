using Api.Models.ElFinder;
using Microsoft.EntityFrameworkCore;

namespace Api.Contexts;

public class ElFinderDbContext : DbContext
{
	public ElFinderDbContext(DbContextOptions<ElFinderDbContext> options) : base(options)
	{
	}

	public DbSet<User> User { get; set; } = null!;
}