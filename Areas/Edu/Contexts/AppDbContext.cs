#region

using Api.Areas.Edu.Models;
using Microsoft.EntityFrameworkCore;

#endregion

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

	public DbSet<TruongThongTinNguoiDung> TruongThongTinNguoiDung { get; set; } = null!;

	public DbSet<GiaTriTruongThongTinNguoiDung> GiaTriTruongThongTinNguoiDung { get; set; } = null!;

	public DbSet<KieuNguoiDung> KieuNguoiDung { get; set; } = null!;

	public DbSet<DanhSachTruongThongTinNguoiDungThuocKieuNguoiDung> DanhSachTruongThongTinNguoiDungThuocKieuNguoiDung { get; set; } = null!;

	public DbSet<ClaimTaikhoan> ClaimTaikhoan { get; set; } = null!;

	/// <summary>
	/// </summary>
	/// <param name="builder"></param>
	protected override void OnModelCreating(ModelBuilder builder)
	{
		builder.Entity<CaiDat>(entity => { entity.HasIndex(x => x.Key).IsUnique(); });

		builder.Entity<CanCuocCongDan>(entity =>
									   {
										   entity.HasIndex(x => x.So)
											     .IsUnique();

										   entity.HasOne(x => x.SoYeuLyLich)
											     .WithOne(x => x.CanCuocCongDan)
											     .HasForeignKey<CanCuocCongDan>(x => x.IdSoYeuLyLich);
									   });

		builder.Entity<ChiTietLich>(entity =>
								    {
									    entity.HasOne(x => x.Lich)
										      .WithMany(x => x.ChiTietLich)
										      .HasForeignKey(x => x.IdLich);

									    entity.HasOne(x => x.NguoiDung)
										      .WithMany(x => x.ChiTietLich)
										      .HasForeignKey(x => x.IdNguoiDung);

									    entity.HasOne(x => x.DiemDanh)
										      .WithOne(x => x.ChiTietLich)
										      .HasForeignKey<DiemDanh>(x => x.IdChiTietLich);

									    entity.HasOne(x => x.NguoiThaoTac)
										      .WithMany(x => x.ChiTietLichCoThaoTacLen)
										      .HasForeignKey(x => x.IdNguoiThaoTac);
								    });


		builder.Entity<MonHoc>(entity => { entity.HasIndex(x => x.Ten).IsUnique(); });

		builder.Entity<LopHoc>(entity => { entity.HasIndex(x => x.Ten).IsUnique(); });

		builder.Entity<TaiKhoan>(entity => { entity.HasIndex(x => x.Username).IsUnique(); });

		builder.Entity<NguoiDung>(entity =>
								  {
									  entity
										  .HasOne(x => x.TaiKhoan)
										  .WithOne(x => x.NguoiDung)
										  .HasForeignKey<NguoiDung>(x => x.IdTaiKhoan);

									  entity
										  .HasMany(x => x.GiaTriTruongThongTinNguoiDung)
										  .WithOne(x => x.NguoiDung)
										  .HasForeignKey(x => x.IdNguoiDung);

									  entity
										  .HasOne(x => x.SoYeuLyLich)
										  .WithOne(x => x.NguoiDung)
										  .HasForeignKey<NguoiDung>(x => x.IdSoYeuLyLich);

									  entity
										  .HasMany(x => x.VaiTro)
										  .WithMany(x => x.NguoiDung);
								  });

		builder.Entity<ChungNhan>(entity =>
								  {
									  entity
										  .HasMany(x => x.HocPhan)
										  .WithMany(x => x.ChungNhan);

									  entity
										  .HasOne(x => x.Mon)
										  .WithMany(x => x.ChungNhan)
										  .HasForeignKey(x => x.IdMonHoc);
								  });

		builder.Entity<ClaimTaikhoan>(entity =>
									  {
										  entity
											  .HasOne(x => x.TaiKhoan)
											  .WithMany(x => x.Claims)
											  .HasForeignKey(x => x.IdTaiKhoan);
									  });

		builder.Entity<DanhSachTruongThongTinNguoiDungThuocKieuNguoiDung>(entity =>
																		  {
																			  entity
																				  .HasOne(x => x.KieuNguoiDung)
																				  .WithMany(x => x.DanhSachTruongThongTinNguoiDungThuocKieuNguoiDung)
																				  .HasForeignKey(x => x.IdKieuNguoiDung);

																			  entity
																				  .HasOne(x => x.TruongThongTinNguoiDung)
																				  .WithMany(x => x.DanhSachTruongThongTinNguoiDungThuocKieuNguoiDung)
																				  .HasForeignKey(x => x.IdTruongThongTinNguoiDung);

																			  entity.HasIndex(x => new { x.IdKieuNguoiDung, x.IdTruongThongTinNguoiDung }).IsUnique();
																		  });

		builder.Entity<DiemDanh>(entity =>
								 {
									 entity
										 .HasOne(x => x.ChiTietLich)
										 .WithOne(x => x.DiemDanh)
										 .HasForeignKey<DiemDanh>(x => x.IdChiTietLich);
								 });

		builder.Entity<GiaTriTruongThongTinNguoiDung>(entity =>
													  {
														  entity
															  .HasIndex(x => new { x.IdNguoiDung, x.IdTruongThongTinNguoiDung })
															  .IsUnique();

														  entity
															  .HasOne(x => x.NguoiDung)
															  .WithMany(x => x.GiaTriTruongThongTinNguoiDung)
															  .HasForeignKey(x => x.IdNguoiDung);

														  entity
															  .HasOne(x => x.TruongThongTinNguoiDung)
															  .WithMany(x => x.GiaTri)
															  .HasForeignKey(x => x.IdTruongThongTinNguoiDung);
													  });

		builder.Entity<HocPhan>(entity =>
							    {
								    entity
									    .HasMany(x => x.HocPhanYeuCau)
									    .WithMany(x => x.HocPhanPhuThuoc);

								    entity
									    .HasMany(x => x.ChungNhan)
									    .WithMany(x => x.HocPhan);

								    entity
									    .HasOne(x => x.MonHoc)
									    .WithMany(x => x.HocPhan)
									    .HasForeignKey(x => x.IdMonHoc);

								    entity
									    .HasMany(x => x.LopHoc)
									    .WithOne(x => x.HocPhan)
									    .HasForeignKey(x => x.IdHocPhan);
							    });

		builder.Entity<KieuNguoiDung>(entity =>
									  {
										  entity
											  .HasMany(x => x.DanhSachNguoiDung)
											  .WithOne(x => x.KieuNguoiDung)
											  .HasForeignKey(x => x.IdKieuNguoiDung);

										  entity
											  .HasMany(x => x.DanhSachTruongThongTinNguoiDungThuocKieuNguoiDung)
											  .WithOne(x => x.KieuNguoiDung)
											  .HasForeignKey(x => x.IdKieuNguoiDung);
									  });

		builder.Entity<Lich>(entity =>
							 {
								 entity
									 .HasOne(x => x.Lop)
									 .WithMany(x => x.Lich)
									 .HasForeignKey(x => x.IdLopHoc);

								 entity
									 .HasOne(x => x.PhongHoc)
									 .WithMany(x => x.Lich)
									 .HasForeignKey(x => x.IdPhongHoc);

								 entity
									 .HasMany(x => x.ChiTietLich)
									 .WithOne(x => x.Lich)
									 .HasForeignKey(x => x.IdLich);
							 });

		builder.Entity<LopHoc>(entity =>
							   {
								   entity
									   .HasOne(x => x.HocPhan)
									   .WithMany(x => x.LopHoc)
									   .HasForeignKey(x => x.IdHocPhan);

								   entity
									   .HasMany(x => x.NguoiThamGia)
									   .WithMany(x => x.LopHocThamgia);

								   entity
									   .HasMany(x => x.Lich)
									   .WithOne(x => x.Lop)
									   .HasForeignKey(x => x.IdLopHoc);

								   entity
									   .HasMany(x => x.PhongHoc)
									   .WithMany(x => x.Lop);
							   });

		builder.Entity<PhongHoc>(entity =>
								 {
									 entity
										 .HasMany(x => x.Lop)
										 .WithMany(x => x.PhongHoc);

									 entity
										 .HasMany(x => x.Lich)
										 .WithOne(x => x.PhongHoc)
										 .HasForeignKey(x => x.IdPhongHoc);
								 });

		builder.Entity<SoYeuLyLich>(entity =>
								    {
									    entity
										    .HasOne(x => x.CanCuocCongDan)
										    .WithOne(x => x.SoYeuLyLich)
										    .HasForeignKey<CanCuocCongDan>(x => x.IdSoYeuLyLich);

									    entity
										    .HasOne(x => x.NguoiDung)
										    .WithOne(x => x.SoYeuLyLich)
										    .HasForeignKey<NguoiDung>(x => x.IdSoYeuLyLich);
								    });

		builder.Entity<TaiKhoan>(entity =>
								 {
									 entity
										 .HasMany(x => x.Claims)
										 .WithOne(x => x.TaiKhoan)
										 .HasForeignKey(x => x.IdTaiKhoan);

									 entity
										 .HasOne(x => x.NguoiDung)
										 .WithOne(x => x.TaiKhoan)
										 .HasForeignKey<NguoiDung>(x => x.IdTaiKhoan);
								 });


		builder.Entity<TruongThongTinNguoiDung>(entity =>
											    {
												    entity
													    .HasMany(x => x.GiaTri)
													    .WithOne(x => x.TruongThongTinNguoiDung)
													    .HasForeignKey(x => x.IdTruongThongTinNguoiDung);

												    entity
													    .HasMany(x => x.DanhSachTruongThongTinNguoiDungThuocKieuNguoiDung)
													    .WithOne(x => x.TruongThongTinNguoiDung)
													    .HasForeignKey(x => x.IdTruongThongTinNguoiDung);
											    });
		builder.Entity<VaiTro>(entity =>
							   {
								   entity
									   .HasMany(x => x.NguoiDung)
									   .WithMany(x => x.VaiTro);
							   });
	}
}