using Projects;

var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<DotNetDevWebTips>("webtips")
    //.WithEndpoint("http", endpoint => {
    //    endpoint.IsProxied = false;
    //    endpoint.Port = 9988;
    //})
    .WithDaprSidecar().WithEndpoint();

builder.Build().Run();
