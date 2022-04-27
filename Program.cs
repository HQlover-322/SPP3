using back;
using back.Data;
using back.Entity;
using back.Services;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(); //sss
builder.Services.AddTransient<ToDoService>();

builder.Services.AddTransient<IUserService,UserService>();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));

builder.Services.AddIdentity<User, IdentityRole>(opts =>
{
    opts.Password.RequiredLength = 5;   // ����������� �����
    opts.Password.RequireNonAlphanumeric = false;   // ��������� �� �� ���������-�������� �������
    opts.Password.RequireLowercase = false; // ��������� �� ������� � ������ ��������
    opts.Password.RequireUppercase = false; // ��������� �� ������� � ������� ��������
    opts.Password.RequireDigit = false; // ��������� �� �����)
})
               .AddEntityFrameworkStores<EfDbContex>();

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

app.UseCors(x=> { x.AllowAnyHeader();x.AllowAnyMethod();x.AllowAnyOrigin(); x.WithOrigins("http://127.0.0.1:5500"); x.AllowCredentials(); });

app.UseCookiePolicy(new CookiePolicyOptions
{
    MinimumSameSitePolicy = SameSiteMode.Strict,
    HttpOnly = HttpOnlyPolicy.Always,
    Secure = CookieSecurePolicy.Always
});

app.UseRouting();

app.UseAuthentication();    
app.UseAuthorization();

app.UseMiddleware<JwtMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
