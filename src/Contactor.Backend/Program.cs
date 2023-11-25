using Contactor.Backend;
using Contactor.Backend.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ContactsDbContext>(opts => {
    string? connectionString = builder.Configuration.GetConnectionString("ContactsDatabase");
    if (string.IsNullOrEmpty(connectionString)) {
        throw new InvalidOperationException("Missing connection string 'ContactsDatabase'");
    }

    opts.UseInMemoryDatabase(connectionString);
 });

builder.Services.AddScoped<IContactsRepository, ContactsRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseDeveloperExceptionPage();
    // app.UseMigrationsEndPoint();
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
