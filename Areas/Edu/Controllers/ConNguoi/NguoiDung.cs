using Api.Contexts;
using Api.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace Api.Areas.Api.Controllers;

/// <summary>
/// </summary>
[Area("Api")]
[Route("/[area]/[controller]")]
[ApiController]
public class NguoiDung : ControllerBase
{
    private readonly AppDbContext _context;

    public NguoiDung(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    // GET: api/NguoiDung
    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        var ketQua = await _context.Nguoi
            .Include(x => x.TaiKhoan)
            .Include(x => x.SoYeuLyLich)
            .Include(x => x.TaiKhoan)
            .AsNoTracking()
            .Select(x => new
            {
                x.Id,
                SoYeuLyLich = new
                {
                    x.SoYeuLyLich!.HoVaTen,
                    x.SoYeuLyLich.SinhNgay,
                    x.SoYeuLyLich.DienThoai,
                    x.SoYeuLyLich.Email,
                },
                x.PhanLoai
            })
            .ToArrayAsync(HttpContext.RequestAborted);
        return Ok(ketQua);
    }

    /// <summary>
    /// </summary>
    /// <param name="ten"></param>
    /// <returns></returns>
    public static string TiengViet(string ten)
    {
        Dictionary<string, string> dic = new()
        {
            { "QuanTri", "Quản trị" }, { "GiangVien", "Giảng viên" }, { "HocVien", "Học viên" },
            { "Nguoi", "Người dùng" }
        };
        return dic[ten];
    }

    /// <summary>
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    // GET: api/NguoiDung/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Nguoi>> Get(Guid id)
    {
        var nguoi = await _context.Nguoi.FindAsync(id);

        return nguoi == null ? NotFound() : nguoi;
    }

    /// <summary>
    /// </summary>
    /// <param name="id"></param>
    /// <param name="nguoi"></param>
    /// <returns></returns>
    // PUT: api/NguoiDung/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutNguoi(Guid id, Nguoi nguoi)
    {
        if (id != nguoi.Id) return BadRequest();

        _context.Entry(nguoi).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!NguoiExists(id))
                return NotFound();
            throw;
        }

        return NoContent();
    }

    /// <summary>
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    // DELETE: api/NguoiDung/5
    [HttpDelete]
    public async Task<IActionResult> DeleteNguoi(Guid id)
    {
        try
        {
            var nguoiDung = await _context.Nguoi.Where(x => x.Id == id).Select(x => new Nguoi
            {
                Id = id,
                RowVersion = x.RowVersion
            }).FirstOrDefaultAsync(HttpContext.RequestAborted);

            if (nguoiDung is null)
                return NotFound();

            _context.Entry(nguoiDung).State = EntityState.Unchanged;

            _context.Remove(nguoiDung);

            var change = await _context.SaveChangesAsync(HttpContext.RequestAborted);

            return change != 0 ? NoContent() : NotFound();
        }
        catch (DbUpdateConcurrencyException)
        {
            return new StatusCodeResult(StatusCodes.Status503ServiceUnavailable);
        }
    }

    /// <summary>
    ///     Cập nhật môn theo id
    /// </summary>
    /// <param name="id">Guid</param>
    /// <param name="path">theo cấu trúc fast joson patch</param>
    /// <returns></returns>
    /// <response code="200">Cập nhật môn mới thành công và trả về môn</response>
    [HttpPatch]
    [ProducesResponseType(typeof(NguoiDung), 200)]
    public async Task<IActionResult> Patch([FromQuery] Guid id, [FromBody] JsonPatchDocument<Nguoi> path)
    {
        using IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, HttpContext.RequestAborted);
        await transaction.CreateSavepointAsync("dau", HttpContext.RequestAborted);
        try
        {
            var nguoiDung = await _context.Nguoi.Include(x => x.SoYeuLyLich).FirstOrDefaultAsync(x => x.Id == id, HttpContext.RequestAborted);
            if (nguoiDung is null)
                return NotFound();

            path.ApplyTo(nguoiDung, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _context.SaveChangesAsync(HttpContext.RequestAborted);
            transaction.Commit();

            return Ok(nguoiDung);
        }
        catch (Exception)
        {
            transaction.Rollback();
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private bool NguoiExists(Guid id)
    {
        return (_context.Nguoi?.Any(e => e.Id == id)).GetValueOrDefault();
    }

    public class InfoDTO
    {
        public string? HoVaTen;

        public Guid? Id;
        public string? PhanLoai;
        public string? TaiKhoanDangNhap;
        public DateTime? ThoiGianTao;

        public InfoDTO()
        {
        }

        public InfoDTO(Nguoi x)
        {
            Id = x.Id;
            HoVaTen = x.SoYeuLyLich?.HoVaTen ?? "None";
            TaiKhoanDangNhap = x.TaiKhoan?.TaiKhoanDangNhap ?? "None";
            ThoiGianTao = x.TaiKhoan!.ThoiGianTao;
            PhanLoai = TiengViet(x.PhanLoai);
        }
    }
}