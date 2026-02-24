var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.Sass_Api>("sass-api");

builder.Build().Run();
