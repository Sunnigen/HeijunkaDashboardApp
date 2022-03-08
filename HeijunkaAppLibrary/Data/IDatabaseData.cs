using HeijunkaAppLibrary.Models;
using System;
using System.Collections.Generic;


namespace HeijunkaAppLibrary.Data
{
    /*
     * All functions directly perform CRUD actions.
     */
    public interface IDatabaseData
    {
        bool Logon(string userName, string password);
        void Logoff(string userName);
        void AddNewUser(string userName, string password, AuthenticationLevel level);
        void UpdateUserPassword(string userName, string currentPassword, string newPassword);
        void UpdateUserAuthenticalLevel(string userName, string currentPassword, AuthenticationLevel newLevel);
        void DeleteUser(string userName);
        UserModel GetUser(string userName);


        void CreateQueue(string queueName, string description, int rowNumber);
        void DeleteQueue(string queueName);
        void EnableQueue(string queueName);
        void DisableQueue(string queueName);
        List<QueueModel> FindActiveQueues();
        List<QueueModel> FindAllQueues();
        List<QueueModel> FindInActiveQueues();


        void CreateProduct(string productName,
                             decimal timetoComplete,
                             string description);
        void UpdateProcess(string productName, decimal timetoComplete, string description);
        void UpdateProcess(string productName, string description);
        void UpdateProcess(string productName, decimal timetoComplete);
        void DeleteProduct(string productName);
        List<ProcessModel> GetAllProcesses();
        ProcessModel FindProcess();

        void AddtoKanban(QueueModel queue,   // specific queue
                         UserModel user,
                         ProcessModel process,
                         DateTime startDate,
                         string orderNumber,
                         string notes);
        void AddtoKanban(UserModel user,   // any queue
                         ProcessModel process,
                         DateTime startDate,
                         string orderNumber,
                         string notes);
        void DeletefromKanban(ProcessModel process);
        List<ScheduleDataModel> GetScheduleData(DateTime date);
    }
}
