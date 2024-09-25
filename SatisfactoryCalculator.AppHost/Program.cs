var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.SatisfactoryCalculator_ApiService>("apiservice");

builder.AddProject<Projects.SatisfactoryCalculator_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(apiService);

builder.Build().Run();
