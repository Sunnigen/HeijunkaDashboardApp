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
        public void AddNewUser(string userName,
                               string password,
                               AuthenticationLevel level)
        {
            // Check if User Already Exists within System
            UserModel? userModel = GetUser(userName);

            if (userModel != null)
            {
                DateTime currDate = DateTime.Today;
                string sql = @"insert into users(UserName, Password, RoleLevel, LastLoggedIn)
                               values (@userName, @password, @level, @currDate)";
                _db.SaveData(sql, new { userName, password, level, currDate }, connectionStringName);
            }

        }
        public UserModel GetUser(string userName)
        {
            string sql = @"select Id, UserName, Password, RoleLevel, LastLoggedin
                           from users
                           where Username = @userName";
            return _db.LoadData<UserModel, dynamic>(sql, new { userName }, connectionStringName).First();
        }

        public List<ProcessModel> GetAllProcesses()
        {
            // Get List of Processes/Products to be Used for Staging and Scheduling
            string sql = @"select Id, Name, Duration, Description
                           from processes";
            return _db.LoadData<ProcessModel, dynamic>(sql, new { }, connectionStringName);
        }
        public ProcessModel GetProcessById(int id)
        {
            // Get List of Processes/Products to be Used for Staging and Scheduling
            string sql = @"select Id, Name, Duration, Description
                           from processes
                           where Id = @id";
            return _db.LoadData<ProcessModel, dynamic>(sql, new { id }, connectionStringName).First();
        }

        public ProcessModel GetProcessByName(string processName)
        {
            // Get List of Processes/Products to be Used for Staging and Scheduling
            string sql = @"select Id, Name, Duration, Description
                           from processes
                           where Name = @processName";
            return _db.LoadData<ProcessModel, dynamic>(sql, new { processName }, connectionStringName).First();
        }

        public void DeleteScheduleData(int id)
        {
            string sql = @"delete from heijunka
                           where Id = @id";
            _db.SaveData(sql, new { id }, connectionStringName);
        }

        public void UpdateScheduleData(SFScheduleDataModel data)
        {
            int id = data.Id;
            int userLastModifiedId = 2;
            int processId = GetProcessByName(data.Subject).Id;
            int queueId = data.QueueId;
            string orderNumber = data.OrderNumber;
            DateTime startTime = Convert.ToDateTime(data.StartTime);
            DateTime lastModifiedDate = DateTime.Now;
            bool isComplete = true;
            bool isActive = true;
            string notes = "Updated";

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
            _db.SaveData(sql, new { queueId, userLastModifiedId, processId, orderNumber, lastModifiedDate, startTime, isComplete, isActive, notes, id }, connectionStringName);

        }

        public void InsertScheduleData(SFScheduleDataModel data)
        {
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

            string sql = @"insert into heijunka(QueueId, UserLastModifiedId, ProcessId, OrderNumber, CreatedDate, LastModifiedDate, StartDate, IsComplete, IsActive, Notes)
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
            string dateString = date.ToShortDateString();

            // Get List of Existing Scheduled Processes
            string sql = @"select Id, QueueId, UserLastModifiedId, ProcessId, OrderNumber, CreatedDate, LastModifiedDate, StartDate, IsComplete, IsActive, Notes
                           from heijunka
                           where @date = StartDate";
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

        public void DeleteProduct(string ProductName)
        {
            throw new NotImplementedException();
        }

        public void DeleteQueue(string QueueName)
        {
            throw new NotImplementedException();
        }

        public void DeleteUser(string UserName)
        {
            throw new NotImplementedException();
        }

        public void DisableQueue(string QueueName)
        {
            throw new NotImplementedException();
        }

        public void EnableQueue(string QueueName)
        {
            throw new NotImplementedException();
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

        public ProcessModel FindProduct()
        {
            throw new NotImplementedException();
        }

        public void Logoff(string UserName)
        {
            throw new NotImplementedException();
        }

        public bool Logon(string UserName, string Password)
        {
            throw new NotImplementedException();
        }

        public void UpdateUserAuthenticalLevel(string UserName, string CurrentPassword, AuthenticationLevel NewLevel)
        {
            throw new NotImplementedException();
        }

        public void UpdateUserPassword(string UserName, string CurrentPassword, string NewPassword)
        {
            throw new NotImplementedException();
        }

        public void CreateProduct(string productName, decimal timetoComplete, string description)
        {
            throw new NotImplementedException();
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

        public ProcessModel FindProcess()
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
                           DROP TABLE IF EXISTS Users";

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
    }
}
