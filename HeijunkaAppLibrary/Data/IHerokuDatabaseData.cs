using HeijunkaAppLibrary.Models;

using System;
using System.Collections.Generic;


namespace HeijunkaAppLibrary.Data
{ 
    /*
     * All functions directly perform CRUD actions.
     */
    public interface IHerokuDatabaseData
    {
        void DeleteAllUsers();
        void PrintTables();
        void UpdateQueue(int id, string queueName, string description, bool isActive);
        void InsertQueue(string queueName, string description);
        List<QueueModel> FindActiveQueues();
        List<QueueModel> FindAllQueues();
        List<QueueModel> FindInActiveQueues();


        void InsertProcess(string productName,
                             decimal timetoComplete,
                             string processDescription);
        void UpdateProcess(string productName, decimal timetoComplete, string description);
        void UpdateProcess(string productName, string description);
        void UpdateProcess(string productName, decimal timetoComplete);
        List<ProcessModel> GetAllProcesses();
        List<ScheduleDataModel> GetScheduleData(DateTime date);
        void InsertScheduleData(SFScheduleDataModel data);
        ScheduleDataModel GetScheduleById(int id);
        void UpdateScheduleData(SFScheduleDataModel data);
        void DeleteScheduleData(int id);
        ProcessModel GetProcessById(int id);
        void CreateTables();
        void DeleteTables();
        void CreateHistoryTable();
        List<string> GetHistoryData();
        void UpdateHistoryData(string entry);
    }
}
