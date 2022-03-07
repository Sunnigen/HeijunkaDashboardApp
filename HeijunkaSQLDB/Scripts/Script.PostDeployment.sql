/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
if not exists (select 1 from dbo.Users)
begin
    insert into dbo.Users(UserName, Password, RoleLevel, LastLoggedIn)
    values ('jason.adams', 'test123', 1, '10/21/2021'),
    ('kevin.alvarez', 'test123', 1, '11/21/2021'),
    ('cool.guy', 'test123', 1, '12/21/2021');
end

if not exists(select 1 from dbo.Processes)
begin
    insert into dbo.Processes(Name, Duration, Description)
    values ('StrongEagle 333 Right Wing Winglets', 240, 'Uses 8-Harness, Style #108 Glass, Peel Ply'),
    ('Newsom LightEngine Inlet Bond Panel', 220, 'Uses Plainweave, Graphite Tape, #221 Glass, #494 Surface Master'),
    ('Aegis 880 Fuselage Connectors', 60, 'Uses Satinweave, #221 Glass, Peel Ply'),
    ('Boeing 444 Fuselage Door', 110, 'Uses 8-Harness, Plainweave, #221 Glass'),
    ('StrongEagle 333 Left Wing Winglets', 250, 'Uses 8-Harness, Style #108 Glass, Peel Ply'),
    ('Newsom LightEngine Repair Kit', 25, 'Uses Plainweave, #494 Surface Master');
end

if not exists(select 1 from dbo.Queues)
begin
    insert into dbo.Queues(CreationDate, UserLastModifiedId, UserLastModifiedDate, IsActive, Description, QueueName)
    values ('12/15/2021', '1', '12/15/2021', 1, 'Processing Area #1', 'Cutting Edge 1'),
    ('10/15/2021', '2', '12/15/2021', 1, 'Bldg 94 Column A', 'Cutting Edge 2'),
    ('10/15/2021', '2', '11/29/2021', 1, 'Bldg 94 Column B', 'Cutting Edge 3'),
    ('10/15/2021', '2', '12/2/2021', 1, 'Bldg 94 Column C', 'Cutting Edge 4'),
    ('10/15/2021', '2', '12/15/2021', 0, 'Bldg 5', 'Bldg 44 Cutting Edge 1');
end