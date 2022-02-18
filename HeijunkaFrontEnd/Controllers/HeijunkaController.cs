using Microsoft.AspNetCore.Mvc;
using HeijunkaTest.Models;
using Syncfusion.EJ2.Schedule;
using Syncfusion.EJ2.Navigations;
using HeijunkaFrontEnd.Models;

namespace HeijunkaTest.Controllers
{
    public class HeijunkaController : Controller
    {
        [BindProperty]
        public List<OwnerModel> ownerData { get; set; } = new List<OwnerModel>();
        public List<AppointmentData> appointmentData { get; set; } = new List<AppointmentData>();

        private List<ScheduleView> viewOptions { get; set; } = new List<ScheduleView>() 
        {
            new ScheduleView { Option = Syncfusion.EJ2.Schedule.View.Agenda}
        }; 

        public HeijunkaController()
        {
            // Set to Timeline Day
            ViewBag.view = this.viewOptions;

            // Initialize TreeView
            //TreeViewFieldsSettings treeViewFields = new TreeViewFieldsSettings();
            //treeViewFields.DataSource = GetStagedParts();
            //treeViewFields.HasChildren = "HasChild";
            //treeViewFields.Expanded = "Expanded";
            //treeViewFields.Id = "Id";
            //treeViewFields.ParentID = "PId";
            //treeViewFields.Text = "Name";
            //ViewBag.Fields = treeViewFields;


            //ViewBag.treeDataSource
        }

        public ActionResult TestFunction1(OwnerModel owner)
        {
            return Json(owner);
        }

        public IActionResult Timeline(List<OwnerModel> oData = null, List<AppointmentData> apData = null)
        {
            // Set Queue Column
            string[] resources = new string[] { "Owners" };
            ViewBag.Resources = resources;
            this.ownerData = this.SetOwnerQueues();
            ViewBag.Owners = this.ownerData;

            // Existing Parts in Process Data
            this.appointmentData = GetScheduleData();
            ViewBag.datasource = this.appointmentData;

            // Set Staging Area
            ViewBag.DataSource = GetStagedParts();

            return View(this.ownerData);
            //return View(this.ownerData, this.appointmentData);
        }

        private List<object> GetStagedParts()
        {
            return new List<object>()
            {
                new
                {
                    Id = 1,
                    Name = "Aileron" 
                },
                new 
                { 
                    Id = 2,
                    Name = "Hinge Flap" 
                },
                new 
                { 
                    Id = 3,
                    Name = "Winglet" 
                }
            };


        //    public int Id { get; set; }
        //public string PartName { get; set; }
        //public bool HasChild { get; set; } = false;
        //public int PId { get; set; }
        //public bool Expanded { get; set; } = false;

    }

        private List<OwnerModel> SetOwnerQueues()
        {
            return new List<OwnerModel>()
            {
               new OwnerModel { Id = 1, Text = "Cutting Edge 1", Color = "#ffaa00" },
               new OwnerModel { Id = 2, Text = "Cutting Edge 2", Color = "#f8a398" },
               new OwnerModel { Id = 3, Text = "Cutting Edge 3", Color = "#7499e1" }
            };
        }

        private List<AppointmentData> GetScheduleData()
        {
            List<AppointmentData> appointmentData = new List<AppointmentData>();

            appointmentData.Add(new AppointmentData
            {
                Id = 1,
                OwnerId = 1,
                Subject = "Gathering Wood",
                StartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 9, 30, 0),
                EndTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 10, 30, 0),
            });

            appointmentData.Add(new AppointmentData
            {
                Id = 2,
                OwnerId = 1,
                Subject = "Chopping Wood",
                StartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 10, 30, 0),
                EndTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 11, 45, 0),
            });

            appointmentData.Add(new AppointmentData
            {
                Id = 3,
                OwnerId = 2,
                Subject = "Taking a Nap",
                StartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 12, 0, 0),
                EndTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 14, 0, 0),
            });



            return appointmentData;
        }
    }
}
