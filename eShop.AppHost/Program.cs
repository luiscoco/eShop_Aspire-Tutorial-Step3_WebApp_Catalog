var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder.AddPostgres("postgres")
    .WithImage("ankane/pgvector")
    .WithImageTag("latest")
    .WithLifetime(ContainerLifetime.Persistent);

var catalogDb = postgres.AddDatabase("catalogdb");

var catalogApi = builder.AddProject<Projects.Catalog_API>("catalog-api")
    .WithReference(catalogDb);

var webApp = builder.AddProject<Projects.WebApp>("webapp")
    .WithExternalHttpEndpoints()
    //.WithReference(basketApi)
    .WithReference(catalogApi);
    //.WithReference(orderingApi)
    //.WithReference(rabbitMq).WaitFor(rabbitMq)
    //.WithEnvironment("IdentityUrl", identityEndpoint);

builder.Build().Run();
