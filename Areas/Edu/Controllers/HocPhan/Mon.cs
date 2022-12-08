using System;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Api.Contexts;
using Api.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Swashbuckle.AspNetCore.Filters;
using LopHoc = Api.Areas.Controllers.LopHoc;

namespace Api.Areas.Api.Controllers;

/// <summary>
///     RESTful API của môn
/// </summary>
[Area("Api")]
[ApiController]
[Route("/[area]/[controller]")]
public partial class MonController : ControllerBase
{
    /// <summary>
    /// </summary>
    private readonly AppDbContext _context;

    /// <summary>
    /// </summary>
    /// <param name="context"></param>
    public MonController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// lấy danh sách môn
    /// </summary>
    /// <param name="ids"></param>
    /// <param name="take"></param>
    /// <param name="skip"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] Guid[]? ids, [FromQuery] int take = -1, [FromQuery] int skip = 0)
        => Ok(await Get(DTO.Get.expression, ids, take, skip));

    private async Task<TOutputType[]> Get<TOutputType>(
        Expression<Func<Models.Mon, TOutputType>> expression,
        [FromQuery] Guid[]? ids,
        [FromQuery] int take = -1,
        [FromQuery] int skip = 0)
        where TOutputType : class
    {
        var query = _context.Mon.AsQueryable();

        if (ids is not null && ids.Any())
            query = query.Where(x => ids.Contains(x.Id));

        if (take > -1)
            query = query.Take(take);

        if (skip > 0)
            query = query.Skip(skip);

        return await query.Select(expression).AsNoTracking().ToArrayAsync(HttpContext.RequestAborted);
    }

    [HttpGet]
    [Route("ToiThieu")]
    public async Task<IActionResult> GetToiThieu(
        [FromQuery] Guid[] ids,
        [FromQuery] int take = -1,
        [FromQuery] int skip = 0)
        => Ok(await Get(DTO.expressionToiThieu, ids, take, skip));
    

    /// <summary>
    ///     Tạo môn mới
    /// </summary>
    /// <param name="mon"></param>
    /// <returns></returns>
    /// <response code="200">Tạo môn mới thành công và trả về môn</response>
    [HttpPost]
    [ProducesResponseType(typeof(Mon), 201)]
    public async Task<IActionResult> Post([FromBody] Models.DTO.MonDTO mon)
    {
        await using IDbContextTransaction transaction =
            await _context.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted, HttpContext.RequestAborted);
        await transaction.CreateSavepointAsync("dau", HttpContext.RequestAborted);
        try
        {
            Mon monMoi = new()
            {
                Ten = mon.Ten,
                MieuTa = mon.MieuTa,
                ThoiGianTao = DateTime.Now
            };

            _context.Attach(monMoi);
            await _context.SaveChangesAsync(HttpContext.RequestAborted);
            await transaction.CommitAsync(HttpContext.RequestAborted);

            return CreatedAtAction(nameof(Get), new { id = monMoi.Id }, monMoi);
        }
        catch (Exception)
        {
            transaction.Rollback();
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
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
    [ProducesResponseType(typeof(Mon), 200)]
    public async Task<IActionResult> Patch([FromQuery] Guid id, [FromBody] JsonPatchDocument<Mon> path)
    {
        await using IDbContextTransaction transaction =
            await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, HttpContext.RequestAborted);
        await transaction.CreateSavepointAsync("dau", HttpContext.RequestAborted);
        try
        {
            var mon = await _context.Mon.FirstOrDefaultAsync(x => x.Id == id, HttpContext.RequestAborted);
            if (mon is null)
                return NotFound();

            path.ApplyTo(mon, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _context.SaveChangesAsync(HttpContext.RequestAborted);
            transaction.Commit();

            return Ok(mon);
        }
        catch (Exception)
        {
            transaction.Rollback();
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    ///     Xóa môn
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <response code="402">Xóa thành công môn</response>
    /// <response code="400">không tìm thấy môn</response>
    /// <response code="500">Lỗi</response>
    [HttpDelete]
    [ProducesResponseType(402)]
    [ProducesResponseType(500)]
    [SwaggerResponseHeader(400, "", "", "")]
    public async Task<IActionResult> Delete(Guid id)
    {
        IDbContextTransaction transaction =
            await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, HttpContext.RequestAborted);
        await transaction.CreateSavepointAsync("dau", HttpContext.RequestAborted);
        try
        {
            var mon = _context.Mon.FirstOrDefault(x => x.Id == id);
            if (mon is null) return NotFound();
            _context.Entry(mon).State = EntityState.Deleted;
            await _context.SaveChangesAsync(HttpContext.RequestAborted);
            await transaction.CommitAsync(HttpContext.RequestAborted);
            return Ok();
        }
        catch (Exception)
        {
            return new StatusCodeResult(500);
        }
    }
}