CREATE TABLE [dbo].[Queues]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [CreationDate] DATE NOT NULL, 
    [UserLastModifiedId] INT NOT NULL, 
    [UserLastModifiedDate] DATE NOT NULL, 
    [IsActive] BIT NOT NULL DEFAULT 1, 
    [Description] NVARCHAR(100) NULL, 
    [QueueName] NVARCHAR(50) NOT NULL, 
    CONSTRAINT [FK_Queues_Users] FOREIGN KEY (UserLastModifiedId) REFERENCES Users(Id)
)
