using Caso1.API.Services.Countries;
using Caso1.API.Services.Import;
using Caso1.API.Services.Usgs;
using Caso1.DataAccess.Context;
using Caso1.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Servicios básicos de la API.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

// Entity Framework y SQL Server.
string connectionString = builder.Configuration
    .GetConnectionString("DefaultConnection")!;

builder.Services.AddDbContext<EarthquakeDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

// Inyección de dependencias
builder.Services.AddScoped<IEarthquakeRepository, EarthquakeRepository>();
builder.Services.AddScoped<IEarthquakeImportService, EarthquakeImportService>();
builder.Services.AddHttpClient<IUsgsService, UsgsService>();
builder.Services.AddHttpClient<ICountryService, CountryService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Crea la base de datos e importa los sismos al iniciar.
using (IServiceScope scope = app.Services.CreateScope())
{
    EarthquakeDbContext context = scope.ServiceProvider
        .GetRequiredService<EarthquakeDbContext>();

    await context.Database.EnsureCreatedAsync();

    // EnsureCreatedAsync no modifica el esquema de una base ya existente.
    // El índice cubre la fecha y el lugar para las búsquedas en bases ya existentes.
    await context.Database.ExecuteSqlRawAsync("""
        IF NOT EXISTS (
            SELECT 1
            FROM sys.indexes
            WHERE name = N'IX_Earthquakes_EventTimeUtc_Place'
              AND object_id = OBJECT_ID(N'[dbo].[Earthquakes]'))
        BEGIN
            CREATE INDEX [IX_Earthquakes_EventTimeUtc_Place]
                ON [dbo].[Earthquakes] ([EventTimeUtc] DESC)
                INCLUDE ([Place])
                WITH (MAXDOP = 1, SORT_IN_TEMPDB = ON);
        END
        """);

    bool importOnStartup = builder.Configuration
        .GetValue<bool>("Usgs:ImportOnStartup");

    if (importOnStartup)
    {
        IEarthquakeImportService importService = scope.ServiceProvider
            .GetRequiredService<IEarthquakeImportService>();

        await importService.ImportNewEarthquakesAsync();
    }
}

app.Run();
