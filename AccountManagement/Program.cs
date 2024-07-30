using AccountManagement.Database;
using AccountManagement.Repositories;
using AccountManagement.Services;
using Oracle.ManagedDataAccess.Client;
using System;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<OracleConnection>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    return new OracleConnection(connectionString);
});
builder.Services.AddSingleton<DatabaseContext>();
builder.Services.AddScoped<userService>();
builder.Services.AddScoped<BaseRepository>();
builder.Services.AddScoped<AccountRepository>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
