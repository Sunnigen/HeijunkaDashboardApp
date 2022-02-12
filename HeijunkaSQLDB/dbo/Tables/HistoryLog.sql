CREATE TABLE [dbo].[HistoryLog]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ProcessId] INT NOT NULL, 
    [ChangeLog] NVARCHAR(200) NULL, 
    CONSTRAINT [FK_HistoryLog_Heijunka] FOREIGN KEY (ProcessId) REFERENCES Heijunka(Id)
)
