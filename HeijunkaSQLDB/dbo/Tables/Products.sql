CREATE TABLE [dbo].[Products]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ProductName] NVARCHAR(50) NOT NULL, 
    [TimetoComplete] DECIMAL(18, 2) NOT NULL, 
    [Description] NVARCHAR(100) NULL
)
