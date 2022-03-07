using Microsoft.AspNetCore.Mvc;
using HeijunkaTest.Models;
using Syncfusion.EJ2.Schedule;
using Syncfusion.EJ2.Navigations;
using HeijunkaFrontEnd.Models;
using HeijunkaAppLibrary.Data;
using HeijunkaAppLibrary.Models;

namespace HeijunkaTest.Controllers
{
    public class HeijunkaController : Controller
    {
        private IDatabaseData _db;

        public HeijunkaController(IDatabaseData db)
        {
            _db = db;  // dependency injection for database queries
        }

        public ActionResult GetQueues()
        {   // Get A List of all ACTIVE Queues
            List<QueueModel> q = _db.FindActiveQueues();
            return Json(q);
        }

        public ActionResult GetProcesses()
        {
            // Get A List of all Processes
            List<ProcessModel> p = _db.GetAllProcesses();
            return Json(p);
        }

        public ActionResult TestFunction1()
        {
            return Json(SetOwnerQueues());
        }

        public IActionResult Timeline()
        {
            // View
            ViewBag.view = new ScheduleView { Option = Syncfusion.EJ2.Schedule.View.Agenda };

            // Set Queue Column
            ViewBag.Resources = new string[] { "Owners" };
            
            // Existing Parts in Process Data
            ViewBag.Appointments = GetScheduleData();

            // Owners/Queues
            List<QueueModel> queues = _db.FindActiveQueues();
            Random rnd = new Random();
            foreach (QueueModel q in queues) {
                string hexOutput = String.Format("{0:X}", rnd.Next(0, 0xFFFFFF));
                while (hexOutput.Length < 6)
                {
                    hexOutput = "0" + hexOutput;
                }
                q.Color = "#" + hexOutput;
            }
        
    
            ViewBag.Owners = queues;

            // Set Staging Area
            ViewBag.DataSource = GetStagedParts();

            // Set Parts
            //ViewBag.Parts = CreateParts();
            ViewBag.Parts = _db.GetAllProcesses();
            List<string> names = new List<string>();
            foreach (ProcessModel m in ViewBag.Parts)
            {
                names.Add(m.Name);  
            }
            ViewBag.PartNames = names;

            // Staging Area Menu Options
            ViewBag.menuOptions = CreateMenuOptions();

            return View();
        }

        private List<object> CreateMenuOptions()
        {
            return new List<object>()
            {
                new
                {
                    text = "Stage Part"
                },
                new
                {
                    text = "Remove Part"
                }
            };
        }

        //private List<PartModel> CreateParts()
        //{
        //    return new List<PartModel>()
        //    {   
        //        new PartModel
        //        {
        //            Id = 1,
        //            Name = "StrongEagle 333 Right Wing Winglets",
        //            Duration = 240
        //        },
        //        new PartModel
        //        {
        //            Id = 2,
        //            Name = "Newsom LightEngine Inlet Bond Panel",
        //            Duration = 220,
        //        },
        //        new PartModel
        //        {
        //            Id = 3,
        //            Name = "Aegis 880 Fuselage Connectors",
        //            Duration = 60,
        //        },
        //        new PartModel
        //        {
        //            Id = 4,
        //            Name = "Rib Bracket",
        //            Duration = 75,
        //        },
        //        new PartModel
        //        {
        //            Id = 5,
        //            Name = "Winglet",
        //            Duration = 65,
        //        },
        //        new PartModel
        //        {
        //            Id = 7,
        //            Name = "Shear Web",
        //            Duration = 30,
        //        }
        //    };
        //}

        private List<StagingObjectModel> GetStagedParts()
        {

            return new List<StagingObjectModel>() {
                new StagingObjectModel
                {
                    Id = 1,
                    Name = "StrongEagle 333 Left Wing Winglets",
                    OrderNumber = "33334444"
                },
                new StagingObjectModel
                {
                    Id = 2,
                    Name = "Newsom LightEngine Repair Kit",
                    OrderNumber = "11112222"
                },
                new StagingObjectModel
                {
                    Id = 3,
                    Name = "Boeing 444 Fuselage Door",
                    OrderNumber = "12345678"
                }
            };
    }

        private List<OwnerModel> SetOwnerQueues()
        {
            return new List<OwnerModel>()
            {
               new OwnerModel { Id = 1, QueueName = "Cutting Edge 1", Color = "#ffaa00" },
               new OwnerModel { Id = 2, QueueName = "Cutting Edge 2", Color = "#f8a398" },
               new OwnerModel { Id = 3, QueueName = "Cutting Edge 3", Color = "#7499e1" }
            };
        }

        private List<AppointmentData> GetScheduleData()
        {
            List<AppointmentData> appointmentData = new List<AppointmentData>();

            appointmentData.Add(new AppointmentData
            {
                Id = 1,
                OwnerId = 1,
                PartModel = new PartModel { Id = 1, Name = "Aileron", Duration = 60},
                StartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9, 30, 0),
                
            });

            appointmentData.Add(new AppointmentData
            {
                Id = 2,
                OwnerId = 1,
                PartModel = new PartModel { Id = 1, Name = "Wing Skin", Duration = 45 },
                StartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 10, 30, 0),
                
            });

            appointmentData.Add(new AppointmentData
            {
                Id = 3,
                OwnerId = 2,
                PartModel = new PartModel { Id = 1, Name = "Hinge Flap", Duration = 25 },
                StartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 12, 0, 0),
            });

            return appointmentData;
        }
    }
}
