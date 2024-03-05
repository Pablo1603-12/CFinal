using Banco.Servicios;
using DAL.Entidades;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<BancoContext>(options =>
     options.UseNpgsql(builder.Configuration.GetConnectionString("ConexionBBDD")));


builder.Services.AddScoped<IUsuarioServicio, UsuarioServicioImpl>();
builder.Services.AddScoped<IServicioEncriptar, ServicioEncriptarImpl>();
builder.Services.AddScoped<IConvertirAdto, ConvertirAdtoImpl>();
builder.Services.AddScoped<IConvertirAdao, ConvertirAdaoImpl>();
builder.Services.AddScoped<IServicioEmail, ServicioEmailImpl>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/auth/login";
});

var app = builder.Build();

// Llama al método para crear el usuario administrador
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var usuarioServicio = services.GetRequiredService<IUsuarioServicio>();
    usuarioServicio.CrearUsuarioAdmin();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
