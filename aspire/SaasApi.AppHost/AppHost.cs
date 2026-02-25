var builder = DistributedApplication.CreateBuilder(args);

builder.AddDockerComposeEnvironment("compose");

var postgres = builder.AddPostgres("postgres")
    .WithPgAdmin(containerName: "PgAdmin")
    .WithDataVolume()
    .WithLifetime(ContainerLifetime.Persistent);

var db = postgres.AddDatabase("saasdb");

var seq = builder.AddSeq("seq");

var redis = builder.AddRedis("redis")
    .WithRedisInsight(containerName: "RedisInsight")
    .WithDataVolume()
    .WithExternalHttpEndpoints();

builder.AddProject<Projects.Saas_Api>("sass-api")
    .WithReference(db)
    .WaitFor(db)
    .WithReference(seq)
    .WaitFor(seq)
    .WithReference(redis)
    .WaitFor(redis);

await builder.Build().RunAsync();
