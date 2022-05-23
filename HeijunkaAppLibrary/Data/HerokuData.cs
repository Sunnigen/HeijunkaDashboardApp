using HeijunkaAppLibrary.Databases;
using HeijunkaAppLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HeijunkaAppLibrary.Data
{
    public class HerokuData : IHerokuDatabaseData
    {
        private readonly IHerokuDataAccess _db;
        private const string connectionStringName = "DATABASE_URL";

        public HerokuData(IHerokuDataAccess db)
        {
            _db = db;
        }
        public List<DataEntryModel> GetHistoryData()
        {
            // Get Entire History of User Changes
            string sql = @"SELECT Id, DataEntry FROM history";
            return _db.LoadData<DataEntryModel, dynamic>(sql, new { }, connectionStringName);
        }
        public void UpdateHistoryData(string data)
        {
            // Get Entire History of User Changes
            string sql = @"INSERT INTO history(DataEntry) values(@data)";
            _db.SaveData(sql, new { data }, connectionStringName);
        }
        public List<ProcessModel> GetAllProcesses()
        {
            // Get List of Processes/Products to be Used for Staging and Scheduling
            string sql = @"SELECT Id, Name, Duration, Description
                           FROM processes";
            return _db.LoadData<ProcessModel, dynamic>(sql, new { }, connectionStringName);
        }
        public ProcessModel GetProcessById(int id)
        {
            // Get List of Processes/Products to be Used for Staging and Scheduling
            string sql = @"SELECT Id, Name, Duration, Description
                           FROM processes
                           WHERE Id = @id";
            return _db.LoadData<ProcessModel, dynamic>(sql, new { id }, connectionStringName).First();
        }

        public ProcessModel GetProcessByName(string processName)
        {
            // Get List of Processes/Products to be Used for Staging and Scheduling
            string sql = @"SELECT Id, Name, Duration, Description
                           FROM processes
                           WHERE Name = @processName";
            return _db.LoadData<ProcessModel, dynamic>(sql, new { processName }, connectionStringName).First();
        }

        public void DeleteScheduleData(int id)
        {
            string sql = @"DELETE FROM heijunka
                           WHERE Id = @id";
            _db.SaveData(sql, new { id }, connectionStringName);
        }

        public void UpdateScheduleData(SFScheduleDataModel data)
        {
            Console.WriteLine("\nUpdateScheduleData");
            int id = data.Id;
            int userLastModifiedId = 2;
            int processId = GetProcessByName(data.Subject).Id;
            int queueId = data.QueueId;
            string orderNumber = data.OrderNumber;
            DateTime startTime = Convert.ToDateTime(data.StartTime);
            DateTime lastModifiedDate = DateTime.Now;
            string notes = "Updated";

            bool isComplete = false;
            bool isActive = false;

            if (data.Status == "Not Started")
            {
                isComplete = false;
                isActive = false;
            } else if (data.Status == "In Work")
            {
                isComplete = false;
                isActive = true;
            } else if (data.Status == "Complete")
            {
                isComplete = true;
                isActive = true;
            }



            string sql = @"update heijunka
                           set QueueId = @queueId, 
                               UserLastModifiedId = @userLastModifiedId,
                               ProcessId = @processId, 
                               OrderNumber = @orderNumber, 
                               LastModifiedDate = @lastModifiedDate, 
                               StartDate = @startTime, 
                               IsComplete = @isComplete, 
                               IsActive = @isActive, 
                               Notes = @notes
                           where Id = @id";
            Console.WriteLine(sql);
            _db.SaveData(sql, new { queueId, userLastModifiedId, processId, orderNumber, lastModifiedDate, startTime, isComplete, isActive, notes, id }, connectionStringName);

        }

        public void InsertScheduleData(SFScheduleDataModel data)
        {
            Console.WriteLine("\nInsertScheduleData");
            Console.WriteLine(data);
            int userLastModifiedId = 1;
            int processId = GetProcessByName(data.Subject).Id;
            int queueId = data.QueueId;
            string orderNumber = data.OrderNumber;
            DateTime startTime = data.StartTime;
            DateTime createdDate = DateTime.Now;
            DateTime lastModifiedDate = DateTime.Now;
            bool isComplete = false;
            bool isActive = false;
            string notes = "Test";

            string sql = @"INSERT INTO heijunka(QueueId, UserLastModifiedId, ProcessId, OrderNumber, CreatedDate, LastModifiedDate, StartDate, IsComplete, IsActive, Notes)
                                             values(@queueId, @userLastModifiedId, @processId, @orderNumber, @createdDate, @lastModifiedDate, @startTime, @isComplete, @isActive, @notes)";
            _db.SaveData(sql, new { queueId, userLastModifiedId, processId, orderNumber, createdDate, lastModifiedDate, startTime, isComplete, isActive, notes }, connectionStringName);
        }

        public ScheduleDataModel GetScheduleById(int id)
        {
            // Get Existing Scheduled Process
            string sql = @"select Id, QueueId, UserLastModifiedId, ProcessId, OrderNumber, CreatedDate, LastModifiedDate, StartDate, IsComplete, IsActive, Notes
                           from heijunka
                           where @id = Id";
            ScheduleDataModel scheduleData = _db.LoadData<ScheduleDataModel, dynamic>(sql, new { id }, connectionStringName).First();
            return scheduleData;
        }

        public List<ScheduleDataModel> GetScheduleData(DateTime date)
        {
            Console.WriteLine("GetScheduleData");
            string dateString = date.ToShortDateString();
            Console.WriteLine($"date: {date}");

            // Get List of Existing Scheduled Processes
            string sql = @"SELECT Id, QueueId, UserLastModifiedId, ProcessId, OrderNumber, CreatedDate, LastModifiedDate, StartDate, IsComplete, IsActive, Notes
                           FROM heijunka
                           WHERE DATE(StartDate) = @date";
            Console.WriteLine($"sql statement: {sql}");
            List<ScheduleDataModel> scheduleData = _db.LoadData<ScheduleDataModel, dynamic>(sql, new { date }, connectionStringName);
            
            // Get Process Data to Obtain Duration

            List<ProcessModel> processList = GetAllProcesses();
            
            var processId = 0;
            foreach (ScheduleDataModel s in scheduleData)
            {
                processId = s.ProcessId;
                foreach (ProcessModel p in processList)
                {
                    if (processId == p.Id)
                    {
                        s.SetEndDate((double)p.Duration);
                        s.StartTime = s.StartDate;
                        s.EndTime = s.StartTime.AddMinutes(p.Duration);
                        s.Subject = p.Name;
                        break;
                    }
                }
            }
            foreach (ScheduleDataModel scheduleDataItem in scheduleData)
            {
                Console.WriteLine($"{scheduleDataItem.Subject} {scheduleDataItem.OrderNumber}");
            }

            return scheduleData;
        }
        public void UpdateQueue(int queueId, string queueName, string description, bool isActive)
        {
            int userLastModifiedId = 1;
            DateTime userLastModifiedDate = DateTime.Today;

            string sql = @"update queues
                            set UserLastModifiedId = @userLastModifiedId,
                                UserLastModifiedDate = @userLastModifiedDate,
                                IsActive = @isActive,
                                Description = @description,
                                QueueName = @queueName
                            where Id = @queueId";
            _db.SaveData(sql, new { userLastModifiedId, userLastModifiedDate, isActive, description, queueName, queueId }, connectionStringName);
        }
        public void InsertQueue(string queueName,
                                string description)
        {
            DateTime date = DateTime.Today;
            int userLastModifiedId = 1;
            DateTime userLastModifiedDate = DateTime.Today;
            bool isActive = true;

            string sql = @"insert into queues(CreationDate, UserLastModifiedId, UserLastModifiedDate, IsActive, Description, QueueName)
                           values (@date, @userLastModifiedId, @userLastModifiedDate, @isActive, @description, @queueName)";
            _db.SaveData(sql, new { date, userLastModifiedId, userLastModifiedDate, isActive, description, queueName}, connectionStringName);
        }
        public List<QueueModel> FindActiveQueues()
        {
            // Find onlys Queues that should be displayed
            string sql = @"select Id, QueueName, Description, IsActive
                           from queues
                           where IsActive = TRUE";
            return _db.LoadData<QueueModel, dynamic>(sql, new { }, connectionStringName);
        }

        public List<QueueModel> FindAllQueues()
        {
            // Find all Queues, both Displayed and Inactive
            string sql = @"select Id, QueueName, Description
                           from queues";
            return _db.LoadData<QueueModel, dynamic>(sql, new { }, connectionStringName);
        }

        public List<QueueModel> FindInActiveQueues()
        {
            // Find all Queues that Shouldn't be Displayed 
            string sql = @"select Id, QueueName, Description
                           from queues
                           where IsActive = FALSE";
            return _db.LoadData<QueueModel, dynamic>(sql, new { }, connectionStringName);
        }

        public void InsertProcess(string productName, decimal timetoComplete, string processDescription)
        {
            string sql = "INSERT into processes(description, duration, name)" +
                         "VALUES (@processDescription, @timetoComplete, @productName)";

            _db.SaveData(sql, new { processDescription, timetoComplete, productName}, connectionStringName);
        }

        public void UpdateProcess(string productName, decimal timetoComplete, string description)
        {
            throw new NotImplementedException();
        }

        public void UpdateProcess(string productName, string description)
        {
            throw new NotImplementedException();
        }

        public void UpdateProcess(string productName, decimal timetoComplete)
        {
            throw new NotImplementedException();
        }
        public void CreateQueueExample()
        {
            string sql = "SELECT table_name FROM information_schema.tables WHERE table_schema = 'public' ORDER BY table_name;";
        }

        public void PrintTables()
        {
            string sql = "SELECT table_name FROM information_schema.tables WHERE table_schema = 'public' ORDER BY table_name;";
            List<string> tables = _db.LoadData<string, dynamic>(sql, new { }, connectionStringName);
            Console.WriteLine("Table Names: ");
            foreach (string table_name in tables)
            {
                Console.WriteLine($"\t{table_name}");
            }
        }

        public void DeleteTables()
        {
            string sql = @"DROP TABLE IF EXISTS Heijunka; 
                           DROP TABLE IF EXISTS HistoryLog; 
                           DROP TABLE IF EXISTS Processes;
                           DROP TABLE IF EXISTS Queues;
                           DROP TABLE IF EXISTS History;
                           DROP TABLE IF EXISTS Users";

            _db.SaveData(sql, new { }, connectionStringName);
        }

        public void DeleteHistoryTable()
        {
            string sql = @"DROP TABLE IF EXISTS History;";

            _db.SaveData(sql, new { }, connectionStringName);
        }

        public void CreateHistoryTable()
        {
            string sql = @"CREATE TABLE history(
    Id serial PRIMARY KEY, 
    DataEntry VARCHAR(1000)
)";

            _db.SaveData(sql, new { }, connectionStringName);

        }

        public void CreateTables()
        {
            string sql = @"CREATE TABLE heijunka(
    Id serial PRIMARY KEY, 
    QueueId INT NOT NULL,
    UserLastModifiedId INT NOT NULL,
    ProcessId INT NOT NULL,
    OrderNumber VARCHAR(10) NOT NULL,
    CreatedDate TIMESTAMP NOT NULL,
    LastModifiedDate TIMESTAMP NOT NULL,
    StartDate TIMESTAMP NOT NULL,
    IsComplete BOOLEAN DEFAULT False NOT NULL,
    IsActive BOOLEAN DEFAULT True NOT NULL,
    Notes VARCHAR(1000)
);

CREATE TABLE historylog(
    Id serial PRIMARY KEY,
    ProcessId INT NOT NULL,
    ChangeLog VARCHAR(200) NULL
);
 
CREATE TABLE processes(
    Id serial PRIMARY KEY,
    Name VARCHAR(50) NOT NULL,
    Duration DECIMAL(18, 2) NOT NULL,
    Description VARCHAR(100) NULL
);

CREATE TABLE queues(
    Id serial PRIMARY KEY,
    CreationDate DATE NOT NULL,
    UserLastModifiedId INT NOT NULL,
    UserLastModifiedDate DATE NOT NULL,
    IsActive BOOLEAN DEFAULT True NOT NULL,
    Description VARCHAR(100) NULL,
    QueueName VARCHAR(50) NOT NULL
);

CREATE TABLE users(
    Id serial PRIMARY KEY,
    UserName VARCHAR(50) NOT NULL,
    Password VARCHAR(50) NOT NULL,
    RoleLevel INT DEFAULT 0 NOT NULL,
    LastLoggedIn DATE NOT NULL
);";

            _db.SaveData(sql, new { }, connectionStringName);
        }

        public void DeleteAllUsers()
        {
            string sql = "DELETE FROM heijunkauser;";
            _db.SaveData(sql, new { }, connectionStringName);

        }
    }
}
