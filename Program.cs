using System.Data;
using NRedisStack;
using Redis.OM;
using RedisApp;
using RedisApp.Services;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var redisUri = builder.Configuration["RedisConnectionString"];
ConfigurationOptions options = new ConfigurationOptions
{
    EndPoints = { redisUri },
    AbortOnConnectFail = false,
    User="default",
    Password=builder.Configuration["RedisPassword"],
    AllowAdmin = true,
   

};
ConnectionMultiplexer connectionMultiplexer= ConnectionMultiplexer.Connect(options);
builder.Services.AddSingleton<IConnectionMultiplexer>(provider =>
      {
          return connectionMultiplexer;
      });


builder.Services.AddSingleton(new RedisConnectionProvider(connectionMultiplexer));
builder.Services.AddHostedService<IndexCreationService>();
builder.Services.AddSingleton<IRedisCacheService, RedisCacheService>();

var app = builder.Build();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
