#region

using Api.Middlewares;
using Api.Tasks;
using Api.ThietLap;

#endregion

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.ThietLapXacThuc();
builder.ThemAppDbContext();
builder.Services.ThietLapMvcjson();
builder.Services.ThietLapCors();
builder.Services.ThietLapSwagger();

WebApplication app = builder.Build();

CheckBackgroundService.Check(app.Services);

if (app.Environment.IsDevelopment())
{
	// app.UseSwagger();
	// app.UseSwaggerUI();
	app.UseOpenApi();
	app.UseSwaggerUi3();
}

app.UseXacThuc();
app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();