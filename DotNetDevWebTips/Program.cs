using Microsoft.AspNetCore.Mvc;
using Serilog.Events;
using Serilog;
using LogDashboard;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using DotNetDevDB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
//将所有控制器加上ApiController特性,加上后所有控制器(不止接口)都要加rote特性
//[assembly: ApiController]

Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Debug()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
        .Enrich.FromLogContext()
        .WriteTo.Console()
        .WriteTo.File($"{AppContext.BaseDirectory}Log/.log", rollingInterval: RollingInterval.Hour, outputTemplate: "{Timestamp:HH:mm:ss} || {Level} || {SourceContext:l} || {Message} ||{NewLine} {Exception} ||end {NewLine}")
        .CreateLogger();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSerilog();
builder.Services.AddLogDashboard();

// Add services to the container.
//builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("Demo", new OpenApiInfo { Title = "My API", Version = "v1" });
    c.SwaggerDoc("Data", new OpenApiInfo { Title = "My API", Version = "v1" });
});

builder.Services.AddDbContextPool<ModelContext>(
    o => o.UseSqlServer(builder.Configuration.GetConnectionString("ModelContext"))
    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking).EnableThreadSafetyChecks(false)
    .LogTo(message => Log.Logger.Error(message), (p, s) => s != LogLevel.Error)
    );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseLogDashboard();
app.UseSwagger();
app.UseSwaggerUI(delegate (SwaggerUIOptions c) {
    new List<string>() { "Demo", "Data" }.ForEach(delegate (string schema) {
        c.SwaggerEndpoint("/swagger/" + schema + "/swagger.json", "Tips" + "." + schema);
    });
});
//app.MapRazorPages();
//app.MapControllers();//需要在控制器or接口上添加Route特性才能访问
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");//添加route特性优先级高于默认规则
app.MapGet("/", () => "Hello World!");

app.Run();
