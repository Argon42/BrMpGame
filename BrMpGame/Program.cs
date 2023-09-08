using BrMpGame;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerUI;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
ConfigurationManager configuration = builder.Configuration;

builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddAppAuth(configuration);
builder.Services.AddControllers();
builder.Services.AddSwagger();
builder.Services.AddEndpointsApiExplorer();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API v1");
        c.DisplayRequestDuration();
        c.DocExpansion(DocExpansion.None);
        c.EnableValidator(null);
        c.EnableFilter();
        c.ShowExtensions();
        c.SupportedSubmitMethods(SubmitMethod.Get, SubmitMethod.Head, SubmitMethod.Post, SubmitMethod.Put, SubmitMethod.Delete, SubmitMethod.Options, SubmitMethod.Patch);
    });
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("cors");
app.MapControllers();

ApplicationConfiguration.CreateDefaultRolesAndUsers(app.Services);

app.Run();