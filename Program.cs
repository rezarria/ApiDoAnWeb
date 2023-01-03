#region

using Api.BackgroundServices;
using Api.Middlewares;
using Api.ThietLap;

#endregion

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<CheckBackgroundService>();
builder.ThietLapXacThuc();
builder.ThemAppDbContext();
builder.Services.ThietLapMvcjson();
builder.Services.ThietLapCors();
builder.Services.ThietLapSwagger();

WebApplication app = builder.Build();

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