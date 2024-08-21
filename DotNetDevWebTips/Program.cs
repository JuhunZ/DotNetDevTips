using Microsoft.AspNetCore.Mvc;
using Serilog.Events;
using Serilog;
using LogDashboard;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using DotNetDevDB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Hosting;
using System.Text;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
//�����п���������ApiController����,���Ϻ����п�����(��ֹ�ӿ�)��Ҫ��rote����
//[assembly: ApiController]

Log.Logger = new LoggerConfiguration()
        .MinimumLevel.Warning()
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
    c.SwaggerDoc("Demo", new OpenApiInfo { Title = "My API", Version = "v10" });
    c.SwaggerDoc("Data", new OpenApiInfo { Title = "My API", Version = "v10" });
    //var list = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.xml");
    //c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
    Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.xml")
                .ToList().ForEach(f => c.IncludeXmlComments(f, true));
});

builder.Services.AddDbContextPool<ModelContext>(
    o => o.UseSqlServer(builder.Configuration.GetConnectionString("ModelContext"))
    //.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)//������� �����޸ġ�ɾ����Ҫ�ֶ�����׷�� 
    .EnableThreadSafetyChecks(false)
    .LogTo(message => {
        Log.Logger.Information(message);
    }, (p, s) => {
        return true;// s == LogLevel.Error;
    })
    );

builder.Services.AddHealthChecks();

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

app.Use(async (context, next) => {
    if (context.Request.Method == HttpMethods.Post) {

        context.Request.EnableBuffering();
        using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true)) {
            var requestBody = await reader.ReadToEndAsync();
            // �����������λ�ã��Ա�����м�����Զ�ȡ��
            context.Request.Body.Position = 0;

            var log = new {
                InterfaceAddress = context.Request.Path.Value,
                RequestParameter = requestBody,
                IpAddress = context.Connection.RemoteIpAddress?.ToString()
            };
            context.RequestServices.GetService<ILogger<Program>>().LogWarning(Newtonsoft.Json.JsonConvert.SerializeObject(log));
        }
    }
    await next(context);
});

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
app.MapGet("/a{b}c{d}", () => "ƥ�䵽��");
app.MapHealthChecks("/healthz") ;

Console.WriteLine("��Ŀ������");
app.Run();
