var builder = DistributedApplication.CreateBuilder(args);

builder.AddDockerComposeEnvironment("compose");

var postgres = builder.AddPostgres("postgres")
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

var db = postgres.AddDatabase("saasdb");

var seq = builder.AddSeq("seq");

builder.AddProject<Projects.Saas_Api>("sass-api")
    .WithReference(db)
    .WaitFor(db)
    .WithReference(seq)
    .WaitFor(db);

await builder.Build().RunAsync();
