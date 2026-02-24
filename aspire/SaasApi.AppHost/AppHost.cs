var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Saas_Api>("sass-api");

builder.Build().Run();
