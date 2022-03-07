CREATE TABLE [dbo].[Processes]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [Duration] DECIMAL(18, 2) NOT NULL, 
    [Description] NVARCHAR(100) NULL
)
