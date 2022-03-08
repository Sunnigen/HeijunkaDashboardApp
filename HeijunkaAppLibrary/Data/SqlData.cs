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

        public void AddtoKanban(QueueModel queue,
                                UserModel user,
                                ProcessModel process,
                                DateTime startDate,
                                string orderNumber,
                                string notes)
        {
            int queueId = queue.Id;
            int userLastModifiedId = user.Id;
            int partId = process.Id;
            DateTime createdDate = DateTime.Today;
            DateTime lastModifiedDate = DateTime.Today;
            DateTime endDate = startDate.AddMinutes((double)process.Duration);
            bool isComplete = false;
            bool isActive = true;
            double timetoComplete = process.Duration;


            string sql = @"insert into Heijunka(QueueId, UserLastModifiedId, partId, OrderNumber, CreatedDate, LastModifiedDate, StartDate, EndDate, IsComplete, IsActive, Notes, TimetoComplete)"
                + @"values (@queueId, @userLastModifiedId, @partId, @orderNumber, @createdDate, @lastModifiedDate, @startDate, @endDate, @isComplete, @isActive, @notes, @timetoComplete)";

            _db.SaveData(sql,
                         new { queueId, 
                             userLastModifiedId, 
                             partId, 
                             orderNumber, 
                             createdDate, 
                             lastModifiedDate, 
                             startDate, 
                             endDate, 
                             isComplete, 
                             isActive, 
                             notes, 
                             timetoComplete 
                         },
                         connectionStringName);
                          
        }

        public void AddtoKanban(UserModel user,
                                ProcessModel process,
                                DateTime startDate,
                                string orderNumber,
                                string notes)
        {
            throw new NotImplementedException();
        }

        public void CreateProcess(string ProductName,
                               decimal TimetoComplete,
                               string Description)
        {
            throw new NotImplementedException();
        }
        public List<ProcessModel> GetAllProcesses()
        {
            // Get List of Processes/Products to be Used for Staging and Scheduling
            string sql = @"select Id, Name, Duration, Description
                           from Processes";
            return _db.LoadData<ProcessModel, dynamic>(sql, new { }, connectionStringName);
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

        public void DeletefromKanban(ProcessModel item)
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

        public List<ProcessModel> GetAllProducts()
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
