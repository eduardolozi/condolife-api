using Condolife.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentityModule(builder.Configuration);
builder.Services.AddCondominiumsModule(builder.Configuration);
builder.Services.AddApi(builder.Configuration);

var app = builder.Build();

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseCors("CondolifeWeb CORS Policy");

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
