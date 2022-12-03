using Api.Models;
using Api.Models.DiaChi;
using Api.Models.SoYeuLyLich;
using Microsoft.EntityFrameworkCore;

namespace Api.Contexts;

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
    public DbSet<Mon> Mon { get; set; } = null!;

    /// <summary>
    /// </summary>
    public DbSet<Nguoi> Nguoi { get; set; } = null!;

    /// <summary>
    /// </summary>
    public DbSet<HocVien> HocVien { get; set; } = null!;

    /// <summary>
    /// </summary>
    public DbSet<GiangVien> GiangVien { get; set; } = null!;

    /// <summary>
    /// </summary>
    public DbSet<SoYeuLyLich> SoYeuLyLich { get; set; } = null!;

    /// <summary>
    /// </summary>
    public DbSet<ChungMinh> ChungMinh { get; set; } = null!;

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
    public DbSet<DanhMucMon> DanhSachMon { get; set; } = null!;

    /// <summary>
    /// </summary>
    public DbSet<KhoaHoc> KhoaHoc { get; set; } = null!;

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
    public DbSet<DiaChi> DiaChi { get; set; } = null!;

    /// <summary>
    /// </summary>
    public DbSet<Tinh> Tinh { get; set; } = null!;

    /// <summary>
    /// </summary>
    public DbSet<QuanHuyen> QuanHuyen { get; set; } = null!;

    /// <summary>
    /// </summary>
    public DbSet<QuanTri> QuanTri { get; set; } = null!;

    /// <summary>
    /// </summary>
    public DbSet<TaiKhoan> TaiKhoan { get; set; } = null!;

    public DbSet<ChiTietLich> ChiTietLich { get; set; } = null!;

    public DbSet<CaiDat> CaiDat { get; set; } = null!;

    public DbSet<HocPhan> HocPhan { get; set; } = null!;

    public DbSet<CaHoc> CaHoc { get; set; } = null!;

    public DbSet<PhongHoc> PhongHoc { get; set; } = null!;

    public DbSet<CoSoDaoTao> CoSoDaoTao { get; set; } = null!;

    public DbSet<NhiemVuHoc> NhiemVuHoc { get; set; } = null!;

    public DbSet<NhomNguoi> NhomNguoi { get; set; } = null!;

    /// <summary>
    /// </summary>
    /// <param name="builder"></param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<HocVien>(enity => { enity.HasIndex(x => x.SoYeuLyLichId).IsUnique(); });

        builder.Entity<Mon>(entity =>
        {
            entity.HasIndex(x => x.Ten).IsUnique();
            entity.HasMany(x => x.DanhMucMon).WithMany(x => x.Mon);
        });

        builder.Entity<DanhMucMon>(entity =>
        {
            entity.HasIndex(x => x.Ten).IsUnique();
            entity.HasMany(x => x.Mon).WithMany(x => x.DanhMucMon);
        });

        builder.Entity<KhoaHoc>(entity => { entity.HasIndex(x => x.Ten).IsUnique(); });

        builder.Entity<LopHoc>(entity => { entity.HasIndex(x => x.Ten).IsUnique(); });

        builder.Entity<Tinh>(entity => { entity.HasIndex(x => x.Ten).IsUnique(); });

        builder.Entity<TaiKhoan>(entity => { entity.HasIndex(x => x.TaiKhoanDangNhap).IsUnique(); });

        builder.Entity<ChiTietLich>(entity =>
        {
            entity.HasOne(x => x.Nguoi).WithMany(x => x.ChiTietLich).IsRequired().OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(x => x.Lich).WithMany(x => x.ChiTietLich).OnDelete(DeleteBehavior.Cascade);
            // entity.HasOne(x => x.DiemDanh).WithOne(x => x.ChiTietLich).OnDelete(DeleteBehavior.ClientCascade);
        });

        builder.Entity<Nguoi>(entity =>
        {
            entity.HasOne(x => (TaiKhoan?)x.TaiKhoan).WithOne(x => (Nguoi)x.NguoiDung);
        });
    }
}