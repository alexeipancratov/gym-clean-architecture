using GymManagement.Application;
using GymManagement.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// These conventions are being used by the TranslateResultToActionResultAttribute only.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProblemDetails(); // Adds required services for the UseExceptionHandler.
builder.Services.AddHttpContextAccessor();

// Application services
builder.Services
    .AddApplication()
    .AddInfrastructure();

var app = builder.Build();

app.UseExceptionHandler(); // Adds the global exception handler middleware (RFC 7807 Problem Details for HTTP APIs).
app.AddInfrastructureMiddleware();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
