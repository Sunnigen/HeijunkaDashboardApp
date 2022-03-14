using Microsoft.AspNetCore.Mvc;
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

        public ActionResult GetScheduleData([FromBody] GetParams param)
        {
            // Obtain Existing Parts in Process Data
            var data = _db.GetScheduleData(param.StartDate);
            return Json(data);
        }
         
        [HttpPost]
        public IActionResult UpdateScheduleData([FromBody] EditParams param)
        {
            if (param.action == "insert" || (param.action == "batch" && param.added.Count != 0)) // this block of code will execute while inserting the appointments
            {
                var value = (param.action == "insert") ? param.value : param.added[0];
                //DateTime startTime = Convert.ToDateTime(value.StartTime);
                //DateTime endTime = Convert.ToDateTime(value.EndTime);
                //SFScheduleDataModel appointment = new SFScheduleDataModel()
                //{
                //    Subject = value.Subject,
                //    OrderNumber = value.OrderNumber,
                //    QueueId = value.QueueId,
                //    StartTime = startTime,
                //    EndTime = endTime,
                    
                //    IsAllDay = value.IsAllDay,
                //    StartTimezone = value.StartTimezone,
                //    EndTimezone = value.EndTimezone,
                //    RecurrenceRule = value.RecurrenceRule,
                //    RecurrenceID = value.RecurrenceID,
                //    RecurrenceException = value.RecurrenceException
                //};
                _db.InsertScheduleData(value);
                return Json(value);
            }
            if (param.action == "update" || (param.action == "batch" && param.changed.Count != 0)) // this block of code will execute while updating the appointment
            {
                var value = (param.action == "update") ? param.value : param.changed[0];
                ScheduleDataModel appointment = _db.GetScheduleById(value.Id);
                if (appointment != null)
                {
                    
                    _db.UpdateScheduleData(value);

                };

                return Json(value);

            }
            
            if (param.action == "remove" || (param.action == "batch" && param.deleted.Count != 0)) // this block of code will execute while removing the appointment
            {
                var value = (param.action == "remove") ? param.value : param.deleted[0];
                ScheduleDataModel appointment = _db.GetScheduleById(value.Id);
                if (appointment != null)
                {

                    _db.DeleteScheduleData(value);

                };

                return Json(value);
            }
            
            return Json(null);
        }

        public IActionResult Timeline()
        {
            // View
            ViewBag.view = new ScheduleView { Option = Syncfusion.EJ2.Schedule.View.Agenda };

            // Set Queue Column
            ViewBag.Resources = new string[] { "Owners" };

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
                new { text = "Stage Part" },
                new { text = "Remove Part" }
            };
        }

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
        public class GetParams
        {
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
        }

        public class EditParams
        {
            public string key { get; set; }
            public string action { get; set; }
            public List<SFScheduleDataModel> added { get; set; }
            public List<SFScheduleDataModel> changed { get; set; }
            public List<SFScheduleDataModel> deleted { get; set; }
            public SFScheduleDataModel value { get; set; }
        }
    }
}
