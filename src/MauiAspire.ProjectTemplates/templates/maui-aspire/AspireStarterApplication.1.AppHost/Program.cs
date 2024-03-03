var builder = DistributedApplication.CreateBuilder(args);

#if UseRedisCache
var cache = builder.AddRedis("cache");

#endif
var apiService = builder.AddProject<Projects.AspireStarterApplication__1_ApiService>("apiservice");

#if IncludeWeb
builder.AddProject<Projects.AspireStarterApplication__1_Web>("webfrontend")
#if UseRedisCache
    .WithReference(cache)
#endif
    .WithReference(apiService);

#endif
// MAUI projects are registered differently than other project types, with AddMobileProject. Aspire settings
// that are normally set as environment variables at launch time are in the case of MAUI instead generated
// in the specified MAUI app project directory, in the AspireAppSettings.g.cs file.
builder.AddMobileProject("mauiclient", "../AspireStarterApplication.1")
    .WithReference(apiService);

builder.Build().Run();
