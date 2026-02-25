using Condolife.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentityModule(builder.Configuration);
builder.Services.AddApi(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseCors();

app.Run();
