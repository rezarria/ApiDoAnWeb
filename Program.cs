#region

using Api.Middlewares;
using Api.Settings;
using RezUtility.Middlewares;
using RezUtility.Tasks;
using System.Text;

#endregion

Console.OutputEncoding = Encoding.UTF8;
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

WebApplication app = builder.AddXacThuc()
							.AddDbContext()
							.AddControllerEtc()
							.AddCors()
							.AddSwagger()
							.AddElFinder()
							.AddBackgroundService()
							.Build();

CheckTask.Check(app.Services);

if (app.Environment.IsDevelopment())
{
	// app.UseSwagger();
	// app.UseSwaggerUI();
	app.UseOpenApi();
	app.UseSwaggerUi3();
}

app.UseHttpsRedirection()
   .UseResponseCompression()
   .UseXacThuc()
   .UseCors()
   .UseRouting()
   .UseAuthentication()
   .UseAuthorization()
   .UseMyEndpoints();

app.Run();