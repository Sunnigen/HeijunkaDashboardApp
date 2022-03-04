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
            ViewBag.Owners = SetOwnerQueues();

            // Set Staging Area
            ViewBag.DataSource = GetStagedParts();

            // Set Parts
            ViewBag.Parts = CreateParts();

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

        private List<PartModel> CreateParts()
        {
            return new List<PartModel>()
            {   
                new PartModel
                {
                    Id = 1,
                    Name = "Aileron",
                    Duration = 45
                },
                new PartModel
                {
                    Id = 2,
                    Name = "Wing Skin",
                    Duration = 180,
                },
                new PartModel
                {
                    Id = 3,
                    Name = "Hinge Flap",
                    Duration = 25,
                },
                new PartModel
                {
                    Id = 4,
                    Name = "Rib Bracket",
                    Duration = 75,
                },
                new PartModel
                {
                    Id = 5,
                    Name = "Winglet",
                    Duration = 65,
                },
                new PartModel
                {
                    Id = 7,
                    Name = "Shear Web",
                    Duration = 30,
                }
            };
        }

        private List<StagingObjectModel> GetStagedParts()
        {

            return new List<StagingObjectModel>() {
                new StagingObjectModel
                {
                    Id = 1,
                    Name = "Aileron",
                    OrderNumber = "33334444"
                },
                new StagingObjectModel
                {
                    Id = 2,
                    Name = "Hinge Flap",
                    OrderNumber = "11112222"
                },
                new StagingObjectModel
                {
                    Id = 3,
                    Name = "Winglet",
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
