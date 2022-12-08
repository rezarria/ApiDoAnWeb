using Api.Contexts;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Linq.Expressions;

namespace Api.Areas.Controllers;

[Area("Api")]
[Route("/[area]/[controller]")]
public partial class LopHoc : ControllerBase
{
    private readonly AppDbContext _context;

    private async Task<TOutputType[]> Get<TOutputType>(
        Expression<Func<Models.LopHoc, TOutputType>> expression,
        [FromQuery] Guid[]? ids,
        [FromQuery] int take = -1,
        [FromQuery] int skip = 0)
        where TOutputType : class
    {
        var query = _context.Lop.AsQueryable();

        if (ids is not null && ids.Any())
            query = query.Where(x => ids.Contains(x.Id));

        if (take > -1)
            query = query.Take(take);

        if (skip > 0)
            query = query.Skip(skip);

        return await query.Select(expression).AsNoTracking().ToArrayAsync(HttpContext.RequestAborted);
    }

    public LopHoc(AppDbContext context)
    {
        _context = context;
    }
    
    /// <summary>
    /// Lấy danh sách lớp học
    /// </summary>
    /// <param name="ids"></param>
    /// <param name="take"></param>
    /// <param name="skip"></param>
    /// <returns></returns>
    public async Task<IActionResult> Get(Guid[]? ids, int take = -1, int skip = 0)
        => Ok(await Get(DTO.Get.expression, ids, take, skip));

    /// <summary>
    ///    Tạo lớp học
    /// </summary>
    /// <param name="lopHoc"></param>
    /// <returns></returns>
    /// <response code="201">Khi tạo lớp thành công</response>
    /// <response code="500">Khi có vấn đề</response>
    /// <response code="400">Khi sai thông tin</response>
    [ProducesResponseType(typeof(Models.LopHoc), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [HttpPost]
    public async Task<IActionResult> Post(DTO.Post lopHoc)
    {
        Models.LopHoc lop = lopHoc.Convert();
        ModelState.ClearValidationState(nameof(DTO.Post));
        if (!TryValidateModel(lop, nameof(Models.LopHoc)))
            return BadRequest(ModelState);
        using IDbContextTransaction transaction =
            await _context.Database.BeginTransactionAsync(HttpContext.RequestAborted);
        try
        {
            transaction.CreateSavepoint("begin");
            _context.Lop.Attach(lop);
            await _context.SaveChangesAsync(HttpContext.RequestAborted);
            transaction.Commit();
            return CreatedAtAction(nameof(Get), new { ids = lop.Id.ToString() }, lop);
        }
        catch (Exception)
        {
            transaction.Rollback();
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }

    /// <summary>
    ///    Xoá lớp học theo id
    /// </summary>
    /// <param name="ids">các id đối tượng muốn xoá</param>
    /// <returns></returns>
    /// <response code="204">Khi xoá lớp thành công</response>
    /// <response code="404">Khi không tìm thấy</response>
    /// <response code="400">Khi không có id</response>
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
    [HttpDelete]
    public async Task<IActionResult> Delete(string ids)
    {
        IEnumerable<Guid> array = ids.Split(";").Select(x => new Guid(x));
        if (array.Any())
        {
            var danhSachLop = from x in _context.Lop
                where array.Contains(x.Id)
                select new Models.LopHoc()
                {
                    Id = x.Id,
                    RowVersion = x.RowVersion
                };
            if (!danhSachLop.Any())
                return NotFound();
            await danhSachLop.ForEachAsync(lop => _context.Entry(lop).State = EntityState.Deleted);
            await _context.SaveChangesAsync(HttpContext.RequestAborted);
            await Response.WriteAsJsonAsync(array);
            return NoContent();
        }

        return BadRequest();
    }

    /// <summary>
    ///     Cập nhật lớp học theo id
    /// </summary>
    /// <param name="id">Guid</param>
    /// <param name="path">theo cấu trúc fast joson patch</param>
    /// <returns></returns>
    /// <response code="200">Cập nhật thành công và trả về kết quả</response>
    /// <response code="404">Khi không tìm thấy</response>
    [ProducesResponseType(typeof(Models.LopHoc), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Models.LopHoc), StatusCodes.Status404NotFound)]
    [HttpPatch]
    public async Task<IActionResult> Patch([FromQuery] Guid id, [FromBody] JsonPatchDocument<Models.LopHoc> path)
    {
        await using IDbContextTransaction transaction =
            await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, HttpContext.RequestAborted);
        await transaction.CreateSavepointAsync("dau", HttpContext.RequestAborted);
        try
        {
            var lopHoc = await _context.Lop.FirstOrDefaultAsync(x => x.Id == id, HttpContext.RequestAborted);
            if (lopHoc is null)
                return NotFound();

            path.ApplyTo(lopHoc, ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _context.SaveChangesAsync(HttpContext.RequestAborted);
            transaction.Commit();

            return Ok(lopHoc);
        }
        catch (Exception)
        {
            transaction.Rollback();
            return new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}