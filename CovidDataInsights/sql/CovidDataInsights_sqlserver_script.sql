USE [master]
GO
/****** Object:  Database [CovidDataInsights]    Script Date: 10/11/2023 10:52:58 PM ******/
CREATE DATABASE [CovidDataInsights]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'CovidDataInsights', FILENAME = N'C:\Users\joana\CovidDataInsights.mdf' , SIZE = 401408KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'CovidDataInsights_log', FILENAME = N'C:\Users\joana\CovidDataInsights_log.ldf' , SIZE = 860160KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [CovidDataInsights] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [CovidDataInsights].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [CovidDataInsights] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [CovidDataInsights] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [CovidDataInsights] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [CovidDataInsights] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [CovidDataInsights] SET ARITHABORT OFF 
GO
ALTER DATABASE [CovidDataInsights] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [CovidDataInsights] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [CovidDataInsights] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [CovidDataInsights] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [CovidDataInsights] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [CovidDataInsights] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [CovidDataInsights] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [CovidDataInsights] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [CovidDataInsights] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [CovidDataInsights] SET  ENABLE_BROKER 
GO
ALTER DATABASE [CovidDataInsights] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [CovidDataInsights] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [CovidDataInsights] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [CovidDataInsights] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [CovidDataInsights] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [CovidDataInsights] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [CovidDataInsights] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [CovidDataInsights] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [CovidDataInsights] SET  MULTI_USER 
GO
ALTER DATABASE [CovidDataInsights] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [CovidDataInsights] SET DB_CHAINING OFF 
GO
ALTER DATABASE [CovidDataInsights] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [CovidDataInsights] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [CovidDataInsights] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [CovidDataInsights] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [CovidDataInsights] SET QUERY_STORE = OFF
GO
USE [CovidDataInsights]
GO
/****** Object:  Table [dbo].[__EFMigrationsHistory]    Script Date: 10/11/2023 10:52:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[__EFMigrationsHistory](
	[MigrationId] [nvarchar](150) NOT NULL,
	[ProductVersion] [nvarchar](32) NOT NULL,
 CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY CLUSTERED 
(
	[MigrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[IntegrationData]    Script Date: 10/11/2023 10:52:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[IntegrationData](
	[IntegrationId] [uniqueidentifier] NOT NULL,
	[IntegrationTimestamp] [datetime2](7) NOT NULL,
	[FileName] [nvarchar](35) NOT NULL,
	[NumberOfRows] [int] NOT NULL,
	[RowsIntegrated] [int] NOT NULL,
 CONSTRAINT [PK_IntegrationData] PRIMARY KEY CLUSTERED 
(
	[IntegrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VaccinationData]    Script Date: 10/11/2023 10:52:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VaccinationData](
	[Id] [uniqueidentifier] NOT NULL,
	[IntegrationId] [uniqueidentifier] NOT NULL,
	[Country] [nvarchar](50) NOT NULL,
	[ISO3] [nvarchar](3) NOT NULL,
	[WhoRegion] [nvarchar](5) NOT NULL,
	[DataSource] [nvarchar](9) NOT NULL,
	[DateUpdated] [nvarchar](10) NULL,
	[TotalVaccinations] [bigint] NULL,
	[PersonsVaccinated_1Plus_Dose] [bigint] NULL,
	[TotalVaccinations_Per100] [float] NULL,
	[PersonsVaccinated_1Plus_Dose_Per100] [float] NULL,
	[PersonsLastDose] [bigint] NULL,
	[PersonsLastDosePer100] [float] NULL,
	[VaccinesUsed] [nvarchar](3) NULL,
	[FirstVaccineDate] [nvarchar](10) NULL,
	[NumberVaccinesTypesUsed] [int] NULL,
	[PersonsBoosterAddDose] [bigint] NULL,
	[PersonsBoosterAddDose_Per100] [float] NULL,
 CONSTRAINT [PK_VaccinationData] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[VaccinationMetaData]    Script Date: 10/11/2023 10:52:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[VaccinationMetaData](
	[Id] [uniqueidentifier] NOT NULL,
	[IntegrationId] [uniqueidentifier] NOT NULL,
	[ISO3] [nvarchar](3) NOT NULL,
	[VaccineName] [nvarchar](64) NOT NULL,
	[ProductName] [nvarchar](64) NULL,
	[CompanyName] [nvarchar](50) NULL,
	[AuthorizationDate] [nvarchar](10) NULL,
	[StartDate] [nvarchar](10) NULL,
	[EndDate] [nvarchar](10) NULL,
	[Comment] [nvarchar](10) NULL,
	[DataSource] [nvarchar](9) NOT NULL,
 CONSTRAINT [PK_VaccinationMetaData] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WhoGlobalData]    Script Date: 10/11/2023 10:52:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WhoGlobalData](
	[Id] [uniqueidentifier] NOT NULL,
	[IntegrationId] [uniqueidentifier] NOT NULL,
	[DateReported] [nvarchar](10) NOT NULL,
	[CountryCode] [nvarchar](3) NULL,
	[Country] [nvarchar](65) NOT NULL,
	[WhoRegion] [nvarchar](10) NOT NULL,
	[NewCases] [bigint] NULL,
	[CumulativeCases] [bigint] NULL,
	[NewDeaths] [bigint] NULL,
	[CumulativeDeaths] [bigint] NULL,
 CONSTRAINT [PK_WhoGlobalData] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WhoGlobalTableData]    Script Date: 10/11/2023 10:52:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WhoGlobalTableData](
	[Id] [uniqueidentifier] NOT NULL,
	[IntegrationId] [uniqueidentifier] NOT NULL,
	[Name] [nvarchar](60) NOT NULL,
	[WhoRegion] [nvarchar](25) NULL,
	[CasesCumulativeTotal] [bigint] NULL,
	[CasesCumulativeTotal_Per100000_Population] [float] NULL,
	[CasesNewlyReportedInLast7Days] [int] NULL,
	[CasesNewlyReportedInLast7Days_Per100000_Population] [float] NULL,
	[CasesNewlyReportedInLast24Hours] [int] NULL,
	[DeathsCumulativeTotal] [bigint] NULL,
	[DeathsCumulativeTotal_Per100000_Population] [float] NULL,
	[DeathsNewlyReportedInLast7Days] [int] NULL,
	[DeathsNewlyReportedInLast7Days_Per100000_Population] [float] NULL,
	[DeathsNewlyReportedInLast24Hours] [int] NULL,
 CONSTRAINT [PK_WhoGlobalTableData] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_FileName]    Script Date: 10/11/2023 10:52:59 PM ******/
CREATE NONCLUSTERED INDEX [IX_FileName] ON [dbo].[IntegrationData]
(
	[FileName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_IntegrationId]    Script Date: 10/11/2023 10:52:59 PM ******/
CREATE NONCLUSTERED INDEX [IX_IntegrationId] ON [dbo].[IntegrationData]
(
	[IntegrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Country]    Script Date: 10/11/2023 10:52:59 PM ******/
CREATE NONCLUSTERED INDEX [IX_Country] ON [dbo].[VaccinationData]
(
	[Country] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_IntegrationId]    Script Date: 10/11/2023 10:52:59 PM ******/
CREATE NONCLUSTERED INDEX [IX_IntegrationId] ON [dbo].[VaccinationData]
(
	[IntegrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_VaccinationMetaData_IntegrationId]    Script Date: 10/11/2023 10:52:59 PM ******/
CREATE NONCLUSTERED INDEX [IX_VaccinationMetaData_IntegrationId] ON [dbo].[VaccinationMetaData]
(
	[IntegrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_WhoGlobalData_IntegrationId]    Script Date: 10/11/2023 10:52:59 PM ******/
CREATE NONCLUSTERED INDEX [IX_WhoGlobalData_IntegrationId] ON [dbo].[WhoGlobalData]
(
	[IntegrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_CountryName]    Script Date: 10/11/2023 10:52:59 PM ******/
CREATE NONCLUSTERED INDEX [IX_CountryName] ON [dbo].[WhoGlobalTableData]
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
/****** Object:  Index [IX_IntegrationId]    Script Date: 10/11/2023 10:52:59 PM ******/
CREATE NONCLUSTERED INDEX [IX_IntegrationId] ON [dbo].[WhoGlobalTableData]
(
	[IntegrationId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[VaccinationMetaData]  WITH CHECK ADD  CONSTRAINT [FK_VaccinationMetaData_IntegrationData_IntegrationId] FOREIGN KEY([IntegrationId])
REFERENCES [dbo].[IntegrationData] ([IntegrationId])
GO
ALTER TABLE [dbo].[VaccinationMetaData] CHECK CONSTRAINT [FK_VaccinationMetaData_IntegrationData_IntegrationId]
GO
ALTER TABLE [dbo].[WhoGlobalData]  WITH CHECK ADD  CONSTRAINT [FK_WhoGlobalData_IntegrationData_IntegrationId] FOREIGN KEY([IntegrationId])
REFERENCES [dbo].[IntegrationData] ([IntegrationId])
GO
ALTER TABLE [dbo].[WhoGlobalData] CHECK CONSTRAINT [FK_WhoGlobalData_IntegrationData_IntegrationId]
GO
ALTER TABLE [dbo].[WhoGlobalTableData]  WITH CHECK ADD  CONSTRAINT [FK_WhoGlobalTableData_IntegrationData_IntegrationId] FOREIGN KEY([IntegrationId])
REFERENCES [dbo].[IntegrationData] ([IntegrationId])
GO
ALTER TABLE [dbo].[WhoGlobalTableData] CHECK CONSTRAINT [FK_WhoGlobalTableData_IntegrationData_IntegrationId]
GO
USE [master]
GO
ALTER DATABASE [CovidDataInsights] SET  READ_WRITE 
GO
