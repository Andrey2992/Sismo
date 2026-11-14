IF DB_ID('Caso1EarthquakesDb') IS NULL CREATE DATABASE Caso1EarthquakesDb;
GO
USE Caso1EarthquakesDb;
GO
IF OBJECT_ID('dbo.Earthquakes', 'U') IS NULL
BEGIN
  CREATE TABLE dbo.Earthquakes(
    Id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Earthquakes PRIMARY KEY,
    ExternalId NVARCHAR(80) NOT NULL,
    Magnitude DECIMAL(5,2) NULL,
    Place NVARCHAR(300) NOT NULL,
    EventTimeUtc DATETIME2 NOT NULL,
    UpdatedTimeUtc DATETIME2 NOT NULL,
    Longitude FLOAT NOT NULL,
    Latitude FLOAT NOT NULL,
    DepthKm FLOAT NOT NULL,
    DetailUrl NVARCHAR(500) NOT NULL,
    Status NVARCHAR(30) NOT NULL,
    Tsunami BIT NOT NULL,
    CONSTRAINT UQ_Earthquakes_ExternalId UNIQUE(ExternalId)
  );
  CREATE INDEX IX_Earthquakes_Place ON dbo.Earthquakes(Place);
END
GO
