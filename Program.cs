using Api.ThietLap;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Cors();
builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.ThietLapSwagger();
builder.ThemAppDbContext();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // app.UseSwagger();
    // app.UseSwaggerUI();
	app.UseOpenApi();
    app.UseSwaggerUi3();
}
app.UseHttpsRedirection();
app.UseCors(x => x.AllowAnyOrigin().AllowAnyHeader().WithMethods("DELETE", "PATCH", "GET", "POST"));
app.UseAuthorization();

app.MapControllers();

app.Run();
