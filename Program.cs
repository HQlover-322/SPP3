using back.Data;
using back.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(); //sss
builder.Services.AddTransient<ToDoService>();
builder.Services.AddSwaggerGen();


ConfigurationManager configuration = builder.Configuration;
builder.Services.AddDbContext<EfDbContex>(option=>option.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
    x=>x.MigrationsAssembly(typeof(EfDbContex).Assembly.FullName)
    ));
builder.Services.AddTransient<FileService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

}

app.UseSwagger();
app.UseSwaggerUI(x => { x.DocumentTitle = "ASD"; x.SwaggerEndpoint("/swagger/v1/swagger.json", "name"); x.RoutePrefix = string.Empty; });


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseCors(x=> { x.AllowAnyHeader();x.AllowAnyMethod();x.AllowAnyOrigin(); });

app.UseRouting();

app.UseAuthorization();

app.MapControllers(); //ss

app.Run();
