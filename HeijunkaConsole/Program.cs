using HeijunkaAppLibrary.Data;
using HeijunkaAppLibrary.Databases;
using HeijunkaAppLibrary.Models;
using HeijunkaFrontEnd.Areas.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

class HerokuConsole
{
    public static ServiceProvider serviceProvider;
    static void Main(string[] args)
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        serviceProvider = services.BuildServiceProvider();

        //DeleteTables();

        //CreateTables();  // Only use once

        //PrintTableInfo(); 

        //CreateQueues();

        //CreateParts();

        // DeleteAllUsers();  // DANGEROUS!!!

        //DeleteHistoryTable(); // DANGEROUS!!!

        //CreateHistoryTable(); // DANGEROUS!!!

        PrintTests();

        Console.ReadLine();
    }

    private static void DeleteHistoryTable()
    {
        var _db = serviceProvider.GetService<IHerokuDatabaseData>();
        _db.DeleteHistoryTable();
        Console.WriteLine("Deleting history table.");
    }

    private static void DeleteAllUsers()
    {
        /*DANGEROUS*/
        var _db = serviceProvider.GetService<IHerokuDatabaseData>();
        _db.DeleteAllUsers();
        Console.WriteLine("Deleting all users!");
    }

    private static void CreateParts()
    {
        var _db = serviceProvider.GetService<IHerokuDatabaseData>();
        _db.InsertProcess(processDescription: "RW part, double-decker plane. Requires extra inspection.", productName: "StrongEagle 333 RW Winglets", timetoComplete: 200);
        _db.InsertProcess(processDescription: "Customer commercial jet.", productName: "Newsom LightEngine Inlet", timetoComplete: 120);
        _db.InsertProcess(processDescription: "Fuselage connectors for Bldg 55.", productName: "Aegis 880 Fuselage Connectors", timetoComplete: 30);
        _db.InsertProcess(processDescription: "Legacy Boeing Spares.", productName: "Boeing 444 Fuselage Door", timetoComplete: 90);
        _db.InsertProcess(processDescription: "LW Part, double-decker plane. Requires extra inspection.", productName: "StrongEagle 333 LW Winglets", timetoComplete: 180);
        _db.InsertProcess(processDescription: "Repair kit for Newsom Lightengine R&D.", productName: "Newsom LightEngine Repair Kit", timetoComplete: 30);
        Console.WriteLine("Creating many parts!.");
    }

    private static void CreateQueues()
    {
        var _db = serviceProvider.GetService<IHerokuDatabaseData>();
        _db.InsertQueue(queueName: "Cutting Edge 1", description: "Bldg 1");
        _db.InsertQueue(queueName: "Cutting Edge 2", description: "Bldg 2 Big Hanger");
        _db.InsertQueue(queueName: "Cutting Edge 3", description: "Bldg 3 AeroBreeze");
        _db.InsertQueue(queueName: "Cutting Edge 42", description: "Bldg 42 R&D");
        Console.WriteLine("Creating many queues!.");
    }

    private static void DeleteTables()
    {
        var _db = serviceProvider.GetService<IHerokuDatabaseData>();
        _db.DeleteTables();
        Console.WriteLine("Deleting all tables.");
    }

    private static void PrintTableInfo()
    {
        var _db = serviceProvider.GetService<IHerokuDatabaseData>();

        _db.PrintTables();
    }

    private static void CreateHistoryTable()
    {
        var _db = serviceProvider.GetService<IHerokuDatabaseData>();

        _db.CreateHistoryTable();
        Console.WriteLine("History table created.");
    }

    private static void CreateTables()
    {
        var _db = serviceProvider.GetService<IHerokuDatabaseData>();

        _db.CreateTables();
        Console.WriteLine("Many tables created.");
    }

    private static void PrintTests()
    {
        var _db = serviceProvider.GetService<IHerokuDatabaseData>();

        Console.WriteLine("\nFindAllQueues:");
        foreach(QueueModel q in _db.FindAllQueues())
        {
            Console.WriteLine($"\t{q.QueueName}");
        };

        Console.WriteLine("\n\nGetScheduleData from 01/01/2020:");
        DateTime startDate = new DateTime(2020, 1, 1);

        foreach (ScheduleDataModel s in _db.GetScheduleData(startDate))
        {
            Console.WriteLine($"\t{s.Subject} From: {s.StartDate} {s.StartTime}, To: {s.EndDate} {s.EndTime}");
        }

        Console.WriteLine("\n\nExisting Processes:");
        foreach (ProcessModel s in _db.GetAllProcesses())
        {
            Console.WriteLine($"\n{s.Name}  Total Time: {s.Duration}  Description: {s.Description}");
        }

        //Console.WriteLine("\n\nUsers:");
        //foreach (HeijunkaUser h in _db.GetAllUsers())
        //{
        //    Console.WriteLine($"\n{h.UserName}");
        //}

    }

    private static void ConfigureServices(IServiceCollection services)
    {
        // Dependency Injection
        var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");

        IConfiguration config = builder.Build();

        services.AddSingleton(config);

        services.AddTransient<IHerokuDataAccess, HerokuDataAccess>();
        services.AddTransient<IHerokuDatabaseData, HerokuData>();

    }


}