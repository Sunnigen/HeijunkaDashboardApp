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
                                ProductModel product,
                                DateTime startDate,
                                string orderNumber,
                                string notes)
        {
            int queueId = queue.Id;
            int userLastModifiedId = user.Id;
            int productId = product.Id;
            DateTime createdDate = DateTime.Today;
            DateTime lastModifiedDate = DateTime.Today;
            DateTime endDate = startDate.AddMinutes((double)product.TimetoComplete);
            bool isComplete = false;
            bool isActive = true;
            decimal timetoComplete = product.TimetoComplete;


            string sql = @"insert into Heijunka(QueueId, UserLastModifiedId, ProductId, OrderNumber, CreatedDate, LastModifiedDate, StartDate, EndDate, IsComplete, IsActive, Notes, TimetoComplete)"
                + @"values (@queueId, @userLastModifiedId, @productId, @orderNumber, @createdDate, @lastModifiedDate, @startDate, @endDate, @isComplete, @isActive, @notes, @timetoComplete)";

            _db.SaveData(sql,
                         new { queueId, 
                             userLastModifiedId, 
                             productId, 
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
                                ProductModel product,
                                DateTime startDate,
                                string orderNumber,
                                string notes)
        {
            throw new NotImplementedException();
        }

        public void CreateProduct(string ProductName,
                                  decimal TimetoComplete,
                                  string Description)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public List<QueueModel> FindAllQueues()
        {
            throw new NotImplementedException();
        }

        public List<QueueModel> FindInActiveQueues()
        {
            throw new NotImplementedException();
        }

        public ProductModel FindProduct()
        {
            throw new NotImplementedException();
        }

        public List<ProductModel> GetAllProducts()
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

        public void UpdateProduct(string ProductName, decimal TimetoComplete, string Description)
        {
            throw new NotImplementedException();
        }

        public void UpdateProduct(string ProductName, string Description)
        {
            throw new NotImplementedException();
        }

        public void UpdateProduct(string ProductName, decimal TimetoComplete)
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
    }
}
