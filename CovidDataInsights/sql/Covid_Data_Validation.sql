
-- Validate integration data
SELECT *  FROM IntegrationData ORDER BY IntegrationTimestamp desc;
SELECT COUNT(*)  FROM VaccinationData;
SELECT COUNT(*)  FROM VaccinationMetaData;
SELECT COUNT(*)  FROM WhoGlobalData;
SELECT COUNT(*)  FROM WhoGlobalTableData;

----------------------------------QUERIES FOR GLOBAL data--------------------------------------

----------------------------------CASES--------------------------
--cumulative cases = 771,151,224
WITH FilteredIntegrationData AS (
    SELECT IntegrationId, MAX(IntegrationTimestamp) AS MaxTimestamp
    FROM IntegrationData
    WHERE FileName = 'WHO-COVID-19-global-table-data.csv'
    GROUP BY IntegrationId
)
SELECT SUM(wtd.CasesCumulativeTotal)
FROM [CovidDataInsights].[dbo].[WhoGlobalTableData] wtd
WHERE wtd.Name = 'Global' AND wtd.IntegrationId= (
SELECT IntegrationId
FROM FilteredIntegrationData
WHERE MaxTimestamp = (SELECT MAX(MaxTimestamp) FROM FilteredIntegrationData));--771151224

--new cases last 7 days = 15,353
WITH FilteredIntegrationData AS (
    SELECT IntegrationId, MAX(IntegrationTimestamp) AS MaxTimestamp
    FROM IntegrationData
    WHERE FileName = 'WHO-COVID-19-global-table-data.csv'
    GROUP BY IntegrationId
)
SELECT SUM(wtd.CasesNewlyReportedInLast7Days)
FROM [CovidDataInsights].[dbo].[WhoGlobalTableData] wtd
WHERE wtd.Name = 'Global' AND wtd.IntegrationId= (
SELECT IntegrationId
FROM FilteredIntegrationData
WHERE MaxTimestamp = (SELECT MAX(MaxTimestamp) FROM FilteredIntegrationData));--15353

----------------------------------VACCINATION--------------------------
-- Total vaccine doses administered = 13,513,207,331
WITH FilteredIntegrationData AS (
    SELECT IntegrationId, MAX(IntegrationTimestamp) AS MaxTimestamp
    FROM IntegrationData
    WHERE FileName = 'vaccination-data.csv'
    GROUP BY IntegrationId
)
SELECT SUM(vd.TotalVaccinations)
FROM [CovidDataInsights].[dbo].[VaccinationData] vd
WHERE vd.IntegrationId= (
SELECT IntegrationId
FROM FilteredIntegrationData
WHERE MaxTimestamp = (SELECT MAX(MaxTimestamp) FROM FilteredIntegrationData));--13513207331

-- Total persons vaccinated with at least one dose = 5,593,693,209
WITH FilteredIntegrationData AS (
    SELECT IntegrationId, MAX(IntegrationTimestamp) AS MaxTimestamp
    FROM IntegrationData
    WHERE FileName = 'vaccination-data.csv'
    GROUP BY IntegrationId
)
SELECT SUM(vd.PersonsVaccinated_1Plus_Dose)
FROM [CovidDataInsights].[dbo].[VaccinationData] vd
WHERE vd.IntegrationId= (
SELECT IntegrationId
FROM FilteredIntegrationData
WHERE MaxTimestamp = (SELECT MAX(MaxTimestamp) FROM FilteredIntegrationData));--5593693209

-- Total persons vaccinated with a complete primary series = 5,153,854,296
WITH FilteredIntegrationData AS (
    SELECT IntegrationId, MAX(IntegrationTimestamp) AS MaxTimestamp
    FROM IntegrationData
    WHERE FileName = 'vaccination-data.csv'
    GROUP BY IntegrationId
)
SELECT SUM(vd.PersonsLastDose)
FROM [CovidDataInsights].[dbo].[VaccinationData] vd
WHERE vd.IntegrationId= (
SELECT IntegrationId
FROM FilteredIntegrationData
WHERE MaxTimestamp = (SELECT MAX(MaxTimestamp) FROM FilteredIntegrationData));--5153854296

----------------------------------DEATHS--------------------------
--cumulative deaths = 139
WITH FilteredIntegrationData AS (
    SELECT IntegrationId, MAX(IntegrationTimestamp) AS MaxTimestamp
    FROM IntegrationData
    WHERE FileName = 'WHO-COVID-19-global-table-data.csv'
    GROUP BY IntegrationId
)
SELECT SUM(wtd.DeathsCumulativeTotal)
FROM [CovidDataInsights].[dbo].[WhoGlobalTableData] wtd
WHERE wtd.Name = 'Global' AND wtd.IntegrationId= (
SELECT IntegrationId
FROM FilteredIntegrationData
WHERE MaxTimestamp = (SELECT MAX(MaxTimestamp) FROM FilteredIntegrationData));--6960783

--new deaths last 7 days = 153
WITH FilteredIntegrationData AS (
    SELECT IntegrationId, MAX(IntegrationTimestamp) AS MaxTimestamp
    FROM IntegrationData
    WHERE FileName = 'WHO-COVID-19-global-table-data.csv'
    GROUP BY IntegrationId
)
SELECT SUM(wtd.DeathsNewlyReportedInLast7Days)
FROM [CovidDataInsights].[dbo].[WhoGlobalTableData] wtd
WHERE wtd.Name = 'Global' AND wtd.IntegrationId= (
SELECT IntegrationId
FROM FilteredIntegrationData
WHERE MaxTimestamp = (SELECT MAX(MaxTimestamp) FROM FilteredIntegrationData));--139


----------------------------------QUERIES FOR PORTUGAL data--------------------------------------
----------------------------------CASES QUERIES--------------------------
--cumulative cases = 5,621,015
WITH FilteredIntegrationData AS (
    SELECT IntegrationId, MAX(IntegrationTimestamp) AS MaxTimestamp
    FROM IntegrationData
    WHERE FileName = 'WHO-COVID-19-global-table-data.csv'
    GROUP BY IntegrationId
)
SELECT SUM(wtd.CasesCumulativeTotal)
FROM [CovidDataInsights].[dbo].[WhoGlobalTableData] wtd
WHERE wtd.Name like '%Portugal%' AND wtd.IntegrationId= (
SELECT IntegrationId
FROM FilteredIntegrationData
WHERE MaxTimestamp = (SELECT MAX(MaxTimestamp) FROM FilteredIntegrationData));--5621015

--new cases last 7 days = 0
WITH FilteredIntegrationData AS (
    SELECT IntegrationId, MAX(IntegrationTimestamp) AS MaxTimestamp
    FROM IntegrationData
    WHERE FileName = 'WHO-COVID-19-global-table-data.csv'
    GROUP BY IntegrationId
)
SELECT SUM(wtd.CasesNewlyReportedInLast7Days)
FROM [CovidDataInsights].[dbo].[WhoGlobalTableData] wtd
WHERE wtd.Name like '%Portugal%' AND wtd.IntegrationId= (
SELECT IntegrationId
FROM FilteredIntegrationData
WHERE MaxTimestamp = (SELECT MAX(MaxTimestamp) FROM FilteredIntegrationData));--0


----------------------------------Vaccination QUERIES--------------------------
-- Total vaccine doses administered = 28,196,689
WITH FilteredIntegrationData AS (
    SELECT IntegrationId, MAX(IntegrationTimestamp) AS MaxTimestamp
    FROM IntegrationData
    WHERE FileName = 'vaccination-data.csv'
    GROUP BY IntegrationId
)
SELECT SUM(vd.TotalVaccinations)
FROM [CovidDataInsights].[dbo].[VaccinationData] vd
WHERE vd.Country like '%Portugal%' AND vd.IntegrationId= (
SELECT IntegrationId
FROM FilteredIntegrationData
WHERE MaxTimestamp = (SELECT MAX(MaxTimestamp) FROM FilteredIntegrationData));--28196689

-- Total persons vaccinated with at least one dose = 9,791,341
WITH FilteredIntegrationData AS (
    SELECT IntegrationId, MAX(IntegrationTimestamp) AS MaxTimestamp
    FROM IntegrationData
    WHERE FileName = 'vaccination-data.csv'
    GROUP BY IntegrationId
)
SELECT SUM(vd.PersonsVaccinated_1Plus_Dose)
FROM [CovidDataInsights].[dbo].[VaccinationData] vd
WHERE vd.Country like '%Portugal%' AND vd.IntegrationId= (
SELECT IntegrationId
FROM FilteredIntegrationData
WHERE MaxTimestamp = (SELECT MAX(MaxTimestamp) FROM FilteredIntegrationData));--9791341

-- Total persons vaccinated with a complete primary series = 8,906,354
WITH FilteredIntegrationData AS (
    SELECT IntegrationId, MAX(IntegrationTimestamp) AS MaxTimestamp
    FROM IntegrationData
    WHERE FileName = 'vaccination-data.csv'
    GROUP BY IntegrationId
)
SELECT SUM(vd.PersonsLastDose)
FROM [CovidDataInsights].[dbo].[VaccinationData] vd
WHERE vd.Country like '%Portugal%' AND vd.IntegrationId= (
SELECT IntegrationId
FROM FilteredIntegrationData
WHERE MaxTimestamp = (SELECT MAX(MaxTimestamp) FROM FilteredIntegrationData));--8906354

----------------------------------DEATHS--------------------------
--cumulative deaths = 27,424
WITH FilteredIntegrationData AS (
    SELECT IntegrationId, MAX(IntegrationTimestamp) AS MaxTimestamp
    FROM IntegrationData
    WHERE FileName = 'WHO-COVID-19-global-table-data.csv'
    GROUP BY IntegrationId
)
SELECT SUM(wtd.DeathsCumulativeTotal)
FROM [CovidDataInsights].[dbo].[WhoGlobalTableData] wtd
WHERE wtd.Name like '%Portugal%' AND wtd.IntegrationId= (
SELECT IntegrationId
FROM FilteredIntegrationData
WHERE MaxTimestamp = (SELECT MAX(MaxTimestamp) FROM FilteredIntegrationData));--27424

--new deaths last 7 days = 0
WITH FilteredIntegrationData AS (
    SELECT IntegrationId, MAX(IntegrationTimestamp) AS MaxTimestamp
    FROM IntegrationData
    WHERE FileName = 'WHO-COVID-19-global-table-data.csv'
    GROUP BY IntegrationId
)
SELECT SUM(wtd.DeathsNewlyReportedInLast7Days)
FROM [CovidDataInsights].[dbo].[WhoGlobalTableData] wtd
WHERE wtd.Name like '%Portugal%' AND wtd.IntegrationId= (
SELECT IntegrationId
FROM FilteredIntegrationData
WHERE MaxTimestamp = (SELECT MAX(MaxTimestamp) FROM FilteredIntegrationData));--0