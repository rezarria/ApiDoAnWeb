using Api.Areas.Edu.Models;
using Microsoft.EntityFrameworkCore;

namespace Api.Areas.Edu.Contexts;

/// <summary>
/// </summary>
public class AppDbContext : DbContext
{
    /// <summary>
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// </summary>
    public DbSet<MonHoc> Mon { get; set; } = null!;

    /// <summary>
    /// </summary>
    public DbSet<NguoiDung> NguoiDung { get; set; } = null!;

    /// <summary>
    /// </summary>
    public DbSet<SoYeuLyLich> SoYeuLyLich { get; set; } = null!;

    /// <summary>
    /// </summary>
    public DbSet<QuanHeGiaDinh> QuanHeGiaDinh { get; set; } = null!;

    /// <summary>
    /// </summary>
    public DbSet<QuaTrinhCongTac> QuaTrinhCongTac { get; set; } = null!;

    /// <summary>
    /// </summary>
    public DbSet<QuaTrinhDaoTao> QuaTrinhDaoTao { get; set; } = null!;

    /// <summary>
    /// </summary>
    public DbSet<LopHoc> Lop { get; set; } = null!;

    /// <summary>
    /// </summary>
    public DbSet<Lich> Lich { get; set; } = null!;

    /// <summary>
    /// </summary>
    public DbSet<DiemDanh> DienDanh { get; set; } = null!;

    /// <summary>
    /// </summary>
    public DbSet<TaiKhoan> TaiKhoan { get; set; } = null!;

    public DbSet<ChiTietLich> ChiTietLich { get; set; } = null!;

    public DbSet<CaiDat> CaiDat { get; set; } = null!;

    public DbSet<HocPhan> HocPhan { get; set; } = null!;

    public DbSet<PhongHoc> PhongHoc { get; set; } = null!;

    /// <summary>
    /// </summary>
    /// <param name="builder"></param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<MonHoc>(entity => { entity.HasIndex(x => x.Ten).IsUnique(); });

        builder.Entity<KhoaHoc>(entity => { entity.HasIndex(x => x.Ten).IsUnique(); });

        builder.Entity<LopHoc>(entity => { entity.HasIndex(x => x.Ten).IsUnique(); });

        builder.Entity<TaiKhoan>(entity => { entity.HasIndex(x => x.TaiKhoanDangNhap).IsUnique(); });

        builder.Entity<ChiTietLich>(entity =>
        {
            entity.HasOne(x => x.NguoiDung).WithMany(x => x.ChiTietLich).IsRequired()
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<NguoiDung>(entity =>
        {
            entity.HasOne(x => (TaiKhoan?)x.TaiKhoan).WithOne(x => (NguoiDung)x.NguoiDung);
        });

        builder.Entity<HocPhan>(entity =>
        {
            entity
                .HasMany(x => x.ChungNhan)
                .WithMany(x => x.HocPhan);
            entity
                .HasOne(x => x.MonHoc)
                .WithMany(x => x.HocPhan)
                .HasForeignKey(x => x.IdMon);
        });

        builder.Entity<ChungNhan>(entity => { });
    }
}