using FortalRPAPI.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 🔹 REGISTRAR SERVIÇOS AQUI
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=fortal.db"));

builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 🔹 CONFIGURAR PIPELINE AQUI
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Administradores}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

var allowedIps = new List<string>
{
    "", // COLOQUE SEU IP MANUALMENTE AQUI PARA ACESSAR A PAGINA DE ADMINISTRADORES
    "127.0.0.1",
    "::1"};

app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value?.ToLower();

    // Bloqueia index e tudo relacionado a Administradores
    if (path == "/" || path.StartsWith("/administradores"))
    {
        var remoteIp = context.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                       ?? context.Connection.RemoteIpAddress?.ToString();

        if (!allowedIps.Contains(remoteIp))
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync("Acesso negado.");
            return;
        }
    }

    await next();
});

app.Run();
