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

        public IActionResult Timeline()
        {
            // View
            ViewBag.view = new ScheduleView { Option = Syncfusion.EJ2.Schedule.View.Agenda };

            // Set Queue Column
            ViewBag.Resources = new string[] { "Owners" };

            // Existing Parts in Process Data
            DateTime d = DateTime.Parse("3/8/2022");
            ViewBag.Appointments = _db.GetScheduleData(d);

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
    }
}
