using Microsoft.AspNetCore.Mvc;
using Serilog.Events;
using Serilog;
using LogDashboard;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using DotNetDevDB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
//�����п���������ApiController����,���Ϻ����п�����(��ֹ�ӿ�)��Ҫ��rote����
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
//app.MapControllers();//��Ҫ�ڿ�����or�ӿ������Route���Բ��ܷ���
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");//���route�������ȼ�����Ĭ�Ϲ���
app.MapGet("/", () => "Hello World!");

app.Run();
