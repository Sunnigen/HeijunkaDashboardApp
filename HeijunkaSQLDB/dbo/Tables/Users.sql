CREATE TABLE [dbo].[Users]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [UserName] NVARCHAR(50) NOT NULL, 
    [Password] NVARCHAR(50) NOT NULL, 
    [RoleLevel] INT NOT NULL DEFAULT 0, 
    [LastLoggedIn] DATE NOT NULL
)
