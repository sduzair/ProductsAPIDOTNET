using ProductsAPI.Hubs;
using ProductsAPI.Models;
using ProductsAPI.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<ProductDatabaseSettings>(
    builder.Configuration.GetSection("ProductDatabase"));
builder.Services.AddSingleton<ProductsService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

////Session
//builder.Services.AddDistributedMemoryCache();

//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromSeconds(10);
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true;
//});

////SignalR
//builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

//app.UseSession();

app.MapControllers();
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}");

//app.MapHub<ProductsHub>("productsHub");

app.Run();
