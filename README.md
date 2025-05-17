# ShishaBuilder
"ConnectionStrings": {
"ConnectionStrings": {
  "DefaultConnectionJamal": "Server=shisha.postgres.database.azure.com;Database=ShishaDb;Port=5432;User Id=Jamal;Password=Bezdim123;Ssl Mode=Require;",
  "DefaultConnection":"Server=tcp:hookah-server.database.windows.net,1433;Initial Catalog=HookahDB;Persist Security Info=False;User ID=Admin123;Password=sharaf123!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"
},


localhost
dotnet ef database update --context ShishaBuilder.Core.Data.AppDbContextIdentity ^
--connection "Server=localhost;Initial Catalog=HookahDB;User ID=admin;Password=admin123;Encrypt=True;TrustServerCertificate=True" ^
--project ShishaBuilder.Core --startup-project ShishaBuilder.Web



SQL command for dropping DB:

-- 1. Удаляем все FOREIGN KEY constraints
DECLARE @sql NVARCHAR(MAX) = N'';

SELECT @sql += '
ALTER TABLE [' + sch.name + '].[' + t.name + '] DROP CONSTRAINT [' + fk.name + '];'
FROM sys.foreign_keys fk
JOIN sys.tables t ON fk.parent_object_id = t.object_id
JOIN sys.schemas sch ON t.schema_id = sch.schema_id;

EXEC sp_executesql @sql;

-- 2. Удаляем все пользовательские таблицы
SET @sql = N'';

SELECT @sql += '
DROP TABLE [' + sch.name + '].[' + t.name + '];'
FROM sys.tables t
JOIN sys.schemas sch ON t.schema_id = sch.schema_id
WHERE is_ms_shipped = 0; -- исключить системные таблицы

EXEC sp_executesql @sql;