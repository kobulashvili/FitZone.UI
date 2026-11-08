
using FitZone.Infrastructure.Data;
using FitZone.Application.Mappings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDbContext<FitZoneDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("FitZoneConnection")
    )
);

// AutoMapper registration
builder.Services.AddAutoMapper(
    cfg => { },
    typeof(MappingProfile)
);

var host = builder.Build();

await host.RunAsync();
