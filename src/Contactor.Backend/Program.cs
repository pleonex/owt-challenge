﻿using System.Reflection;
using Contactor.Backend.Models.Domain;
using Contactor.Backend.Models.Dto.Contacts;
using Contactor.Backend.Models.Dto.Skills;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ContactsDbContext>(opts => {
    string? connectionString = builder.Configuration.GetConnectionString("ContactsDatabase");
    if (string.IsNullOrEmpty(connectionString)) {
        throw new InvalidOperationException("Missing connection string 'ContactsDatabase'");
    }

    opts.UseInMemoryDatabase(connectionString);
 });

builder.Services.AddScoped<IContactsRepository, ContactsRepository>()
    .AddScoped<ISkillRepository, SkillRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opts => {
#pragma warning disable S1075 // URIs should not be hardcoded
    opts.SwaggerDoc("v1", new OpenApiInfo {
        Version = "v1",
        Title = "Contacts API",
        Description = "Contacts REST API for the OWT tech challenge",
        TermsOfService = new Uri("https://www.owt.swiss/en/owt-impressum/"),
        Contact = new OpenApiContact {
            Name = "Benito Palacios Sanchez",
            Email = "benito.palsan@protonmail.com",
        },
        License = new OpenApiLicense {
            Name = "OWT license",
            Url = new Uri("https://www.owt.swiss/en/owt-impressum/"),
        },
    });
#pragma warning restore S1075

    var xmlDocs = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    opts.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlDocs));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseDeveloperExceptionPage();
} 

// Create database if it doesn't exist
// TODO: Remove for production and use migrations
using (var scope = app.Services.CreateScope()) {
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<ContactsDbContext>();
    context.Database.EnsureCreated();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
