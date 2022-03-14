using HeijunkaAppLibrary.Databases;
using HeijunkaAppLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HeijunkaAppLibrary.Data
{
    public class SqlData : IDatabaseData
    {
        private readonly ISqlDataAccess _db;
        private const string connectionStringName = "SqlDb";

        public SqlData(ISqlDataAccess db)
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
                string sql = @"insert into Users(UserName, Password, RoleLevel, LastLoggedIn)
                               values (@userName, @password, @level, @currDate)";
                _db.SaveData(sql, new { userName, password, level, currDate }, connectionStringName);
            }

        }
        public UserModel GetUser(string userName)
        {
            string sql = @"select Id, UserName, Password, RoleLevel, LastLoggedin
                           from Users
                           where Username = @userName";
            return _db.LoadData<UserModel, dynamic>(sql, new { userName }, connectionStringName).First();
        }

        public List<ProcessModel> GetAllProcesses()
        {
            // Get List of Processes/Products to be Used for Staging and Scheduling
            string sql = @"select Id, Name, Duration, Description
                           from Processes";
            return _db.LoadData<ProcessModel, dynamic>(sql, new { }, connectionStringName);
        }
        public ProcessModel GetProcessById(int id)
        {
            // Get List of Processes/Products to be Used for Staging and Scheduling
            string sql = @"select Id, Name, Duration, Description
                           from Processes
                           where Id = @id";
            return _db.LoadData<ProcessModel, dynamic>(sql, new { id }, connectionStringName).First();
        }

        public ProcessModel GetProcessByName(string processName)
        {
            // Get List of Processes/Products to be Used for Staging and Scheduling
            string sql = @"select Id, Name, Duration, Description
                           from Processes
                           where Name = @processName";
            return _db.LoadData<ProcessModel, dynamic>(sql, new { processName }, connectionStringName).First();
        }

        public void DeleteScheduleData(SFScheduleDataModel data)
        {
            int id = data.Id;
            string sql = @"delete from dbo.Heijunka
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
            int isComplete = 1;
            int isActive = 1;
            string notes = "Updated";

            string sql = @"update dbo.Heijunka
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
            int isComplete = 0;
            int isActive = 0;
            string notes = "Test";

            string sql = @"insert into dbo.Heijunka(QueueId, UserLastModifiedId, ProcessId, OrderNumber, CreatedDate, LastModifiedDate, StartDate, IsComplete, IsActive, Notes)
                                             values(@queueId, @userLastModifiedId, @processId, @orderNumber, @createdDate, @lastModifiedDate, @startTime, @isComplete, @isActive, @notes)";
            _db.SaveData(sql, new { queueId, userLastModifiedId, processId, orderNumber, createdDate, lastModifiedDate, startTime, isComplete, isActive, notes }, connectionStringName);
        }

        public ScheduleDataModel GetScheduleById(int id)
        {
            // Get Existing Scheduled Process
            string sql = @"select Id, QueueId, UserLastModifiedId, ProcessId, OrderNumber, CreatedDate, LastModifiedDate, StartDate, IsComplete, IsActive, Notes
                           from Heijunka
                           where @id = Id";
            ScheduleDataModel scheduleData = _db.LoadData<ScheduleDataModel, dynamic>(sql, new { id }, connectionStringName).First();
            return scheduleData;
        }

        public List<ScheduleDataModel> GetScheduleData(DateTime date)
        {
            string dateString = date.ToShortDateString();

            // Get List of Existing Scheduled Processes
            string sql = @"select Id, QueueId, UserLastModifiedId, ProcessId, OrderNumber, CreatedDate, LastModifiedDate, StartDate, IsComplete, IsActive, Notes
                           from Heijunka
                           where @dateString = convert(date, StartDate, 110)";
            List<ScheduleDataModel> scheduleData = _db.LoadData<ScheduleDataModel, dynamic>(sql, new { dateString }, connectionStringName);

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

        public void CreateQueue(string QueueName,
                                string Description,
                                int RowNumber)
        {
            throw new NotImplementedException();
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
            string sql = @"select Id, QueueName, Description
                           from Queues
                           where IsActive = 1";
            return _db.LoadData<QueueModel, dynamic>(sql, new { }, connectionStringName);
        }

        public List<QueueModel> FindAllQueues()
        {
            // Find all Queues, both Displayed and Inactive
            string sql = @"select Id, QueueName, Description
                           from Queues";
            return _db.LoadData<QueueModel, dynamic>(sql, new { }, connectionStringName);
        }

        public List<QueueModel> FindInActiveQueues()
        {
            // Find all Queues that Shouldn't be Displayed 
            string sql = @"select Id, QueueName, Description
                           from Queues
                           where IsActive = 0";
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
    }
}
