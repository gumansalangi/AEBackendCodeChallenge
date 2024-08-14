using AEBackendCodeChallenge.Data;
using AEBackendCodeChallenge.Services;
using AEBackendCodeChallenge.Validators;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Add multiple validator
builder.Services.AddControllers()
    .AddFluentValidation(fv =>
    {
        fv.RegisterValidatorsFromAssemblyContaining<UserValidator>();
        fv.RegisterValidatorsFromAssemblyContaining<PortValidator>();
        fv.RegisterValidatorsFromAssemblyContaining<ShipValidator>();
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register the DbContext with dependency injection
builder.Services.AddDbContext<ShipDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register service in dependency injection
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IShipService, ShipService>();
builder.Services.AddScoped<IPortService, PortService>();

var app = builder.Build();

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
