IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

CREATE TABLE [IntegrationData] (
    [IntegrationId] uniqueidentifier NOT NULL,
    [IntegrationTimestamp] datetime2 NOT NULL,
    [FileName] nvarchar(35) NOT NULL,
    [NumberOfRows] int NOT NULL,
    [RowsIntegrated] int NOT NULL,
    CONSTRAINT [PK_IntegrationData] PRIMARY KEY ([IntegrationId])
);
GO

CREATE TABLE [VaccinationData] (
    [Id] uniqueidentifier NOT NULL,
    [IntegrationId] uniqueidentifier NOT NULL,
    [Country] nvarchar(50) NOT NULL,
    [ISO3] nvarchar(3) NOT NULL,
    [WhoRegion] nvarchar(5) NOT NULL,
    [DataSource] nvarchar(9) NOT NULL,
    [DateUpdated] nvarchar(10) NOT NULL,
    [TotalVaccinations] bigint NULL,
    [PersonsVaccinated_1Plus_Dose] bigint NULL,
    [TotalVaccinations_Per100] float NULL,
    [PersonsVaccinated_1Plus_Dose_Per100] float NULL,
    [PersonsLastDose] bigint NULL,
    [PersonsLastDosePer100] float NULL,
    [VaccinesUsed] nvarchar(3) NULL,
    [FirstVaccineDate] nvarchar(10) NULL,
    [NumberVaccinesTypesUsed] int NULL,
    [PersonsBoosterAddDose] bigint NULL,
    [PersonsBoosterAddDose_Per100] float NULL,
    CONSTRAINT [PK_VaccinationData] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_VaccinationData_IntegrationData_IntegrationId] FOREIGN KEY ([IntegrationId]) REFERENCES [IntegrationData] ([IntegrationId]) ON DELETE NO ACTION
);
GO

CREATE INDEX [IX_VaccinationData_IntegrationId] ON [VaccinationData] ([IntegrationId]);
GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20230727123945_Initial', N'6.0.8');
GO

COMMIT;
GO

