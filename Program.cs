#region

using Api.Middlewares;
using Api.Settings;
using Api.Tasks;
using System.Text;

#endregion
Console.OutputEncoding = Encoding.UTF8;
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.AddXacThuc();
builder.DbContext();
builder.Services.AddControllerEtc();
builder.AddCors();
builder.Services.AddSwagger();

WebApplication app = builder.Build();

CheckTask.Check(app.Services);

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