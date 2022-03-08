CREATE TABLE [dbo].[Heijunka]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [QueueId] INT NOT NULL, 
    [UserLastModifiedId] INT NOT NULL, 
    [ProcessId] INT NOT NULL, 
    [OrderNumber] NVARCHAR(10) NOT NULL, 
    [CreatedDate] DATETIME2(0) NOT NULL, 
    [LastModifiedDate] DATETIME2(0) NOT NULL, 
    [StartDate] DATETIME2(0) NOT NULL, 
    [IsComplete] BIT NOT NULL DEFAULT 0, 
    [IsActive] BIT NOT NULL DEFAULT 0, 
    [Notes] NVARCHAR(100) NULL,
    CONSTRAINT [FK_Heijunka_Queues] FOREIGN KEY (QueueId) REFERENCES Queues(Id), 
    CONSTRAINT [FK_Heijunka_Users] FOREIGN KEY (UserLastModifiedId) REFERENCES Users(Id), 
    CONSTRAINT [FK_Heijunka_Processes] FOREIGN KEY (ProcessId) REFERENCES Processes(Id)
)
