CREATE TABLE [dbo].[Heijunka]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [QueueId] INT NOT NULL, 
    [UserLastModifiedId] INT NOT NULL, 
    [ProductId] INT NOT NULL, 
    [OrderNumber] NVARCHAR(10) NOT NULL, 
    [CreatedDate] DATE NOT NULL, 
    [LastModifiedDate] DATE NOT NULL, 
    [StartDate] DATE NOT NULL, 
    [EndDate] DATE NOT NULL, 
    [IsComplete] BIT NOT NULL DEFAULT 0, 
    [IsActive] BIT NOT NULL DEFAULT 0, 
    [Notes] NVARCHAR(100) NULL,
    [TimetoComplete] INT NOT NULL, 
    CONSTRAINT [FK_Heijunka_Queues] FOREIGN KEY (QueueId) REFERENCES Queues(Id), 
    CONSTRAINT [FK_Heijunka_Users] FOREIGN KEY (UserLastModifiedId) REFERENCES Users(Id), 
    CONSTRAINT [FK_Heijunka_Products] FOREIGN KEY (ProductId) REFERENCES Products(Id)
)
