using Microsoft.AspNetCore.Mvc;
using HeijunkaTest.Models;
using Syncfusion.EJ2.Schedule;

namespace HeijunkaTest.Controllers
{
    public class HeijunkaController : Controller
    {
        [BindProperty]
        public List<OwnerModel> ownerData { get; set; } = new List<OwnerModel>();

        public IActionResult Timeline()
        {
            // Set Queue Column
            string[] resources = new string[] { "Owners" };
            ViewBag.Resources = resources;
            this.SetOwnerQueues();
            ViewBag.Owners = ownerData;

            // Existing Parts in Process Data
            //ViewBag.appointments = GetScheduleData();
            ViewBag.datasource = GetScheduleData();

            // Set to Timeline Day
            List<ScheduleView> viewOptions = new List<ScheduleView>()
            {
                new ScheduleView { Option = Syncfusion.EJ2.Schedule.View.TimelineDay }
            };

            ViewBag.view = viewOptions;

            return View();
        }
        private void SetOwnerQueues()
        {
            ownerData.Add(new OwnerModel { Id = 1, Text = "Cutting Edge 1", Color = "#ffaa00" });
            ownerData.Add(new OwnerModel { Id = 2, Text = "Cutting Edge 2", Color = "#f8a398" });
            ownerData.Add(new OwnerModel { Id = 3, Text = "Cutting Edge 3", Color = "#7499e1" });
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
