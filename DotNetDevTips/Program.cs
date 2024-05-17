// See https://aka.ms/new-console-template for more information
using DotNetDevDB;
using Microsoft.EntityFrameworkCore;

Console.WriteLine("Hello, World!");
//using var db = new ModelContext();//此方法需要修改modelcontext 与web方式不兼容
var contextOptions = new DbContextOptionsBuilder<ModelContext>()
    .UseSqlServer(@"Data Source=127.0.0.1,1433;Initial Catalog=Demo;Persist Security Info=True;User ID=sa;password=123456;TrustServerCertificate=true")
    .Options;

using var db = new ModelContext(contextOptions);


Console.WriteLine($"第一个学生的名称：{db.Students.First().Name}");

Console.ReadLine();