using HeijunkaAppLibrary.Data;
using HeijunkaAppLibrary.Databases;
using HeijunkaAppLibrary.Models;
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

        CreateQueues();



        PrintTests();

        Console.ReadLine();
    }

    private static void CreateQueues()
    {
        var _db = serviceProvider.GetService<IHerokuDatabaseData>();
        _db.InsertQueue(queueName: "Cutting Edge 1", description: "Bldg 1");
        _db.InsertQueue(queueName: "Cutting Edge 2", description: "Bldg 2 Big Hanger");
        _db.InsertQueue(queueName: "Cutting Edge 3", description: "Bldg 3 AeroBreeze");
        _db.InsertQueue(queueName: "Cutting Edge 42", description: "Bldg 42 R&D");
    }

    private static void DeleteTables()
    {
        var _db = serviceProvider.GetService<IHerokuDatabaseData>();
        _db.DeleteTables();
    }

    private static void PrintTableInfo()
    {
        var _db = serviceProvider.GetService<IHerokuDatabaseData>();

        _db.PrintTables();
    }

    private static void CreateTables()
    {
        var _db = serviceProvider.GetService<IHerokuDatabaseData>();

        _db.CreateTables();
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