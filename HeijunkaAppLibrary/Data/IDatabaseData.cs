using HeijunkaAppLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace HeijunkaAppLibrary.Data
{
    public interface IDatabaseData
    {
        bool Logon(string UserName, string Password);
        void Logoff(string UserName);
        void AddNewUser(string UserName, string Password, AuthenticationLevel Level);
        void UpdateUserPassword(string UserName, string CurrentPassword, string NewPassword);
        void UpdateUserAuthenticalLevel(string UserName, string CurrentPassword, AuthenticationLevel NewLevel);
        void DeleteUser(string UserName);


        void CreateQueue(string QueueName, string Description, int RowNumber);
        void DeleteQueue(string QueueName);
        void EnableQueue(string QueueName);
        void DisableQueue(string QueueName);
        List<QueueModel> FindActiveQueues();
        List<QueueModel> FindAllQueues();
        List<QueueModel> FindInActiveQueues();


        void CreateProduct(string ProductName, decimal TimetoComplete, string Description);
        void UpdateProduct(string ProductName, decimal TimetoComplete, string Description);
        void UpdateProduct(string ProductName, string Description);
        void UpdateProduct(string ProductName, decimal TimetoComplete);
        void DeleteProduct(string ProductName);
        List<ProductModel> GetAllProducts();
        ProductModel FindProduct();



    }
}
