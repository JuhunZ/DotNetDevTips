// See https://aka.ms/new-console-template for more information

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using DotNetDevDB;
using DotNetDevWebTips.Apis;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

Console.WriteLine("Hello, World!");
var summary = BenchmarkRunner.Run(typeof(Program).Assembly);

Console.ReadLine();



public class BenchClass {
    static DbContextOptions<ModelContext> contextOptions = new DbContextOptionsBuilder<ModelContext>()
  .UseSqlServer(@"Data Source=127.0.0.1,1433;Initial Catalog=Demo;Persist Security Info=True;User ID=sa;password=123456;TrustServerCertificate=true")
  .Options;

    public BenchClass() {
    }

    [Benchmark]
    public async Task Test1() {
        using var db = new ModelContext(contextOptions);
        var result = await new DataController().GetCompile(db);
    }

    [Benchmark]
    public async Task Test2() {
        using var db = new ModelContext(contextOptions);
        var result = await new DataController().GetCommon(db);
        
    }
}
