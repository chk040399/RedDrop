// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Reflection;
using BD.MigrationService;
using BD.PublicPortal.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<ApiDbInitializer>();

builder.AddServiceDefaults();

builder.Services.AddDbContextPool<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("PublicPortalDatabase"), sqlOptions =>
        sqlOptions.MigrationsAssembly(Assembly.GetExecutingAssembly())
    ));

builder.EnrichNpgsqlDbContext<AppDbContext>();

var app = builder.Build();

app.Run();
