using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using TakeHomeChallenge.Models;
using TakeHomeChallenge.Services;

namespace TakeHomeChallenge.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CsvDataService csvDataService;

        public HomeController(ILogger<HomeController> logger,
            CsvDataService employeeDataService)
        {
            this.csvDataService = employeeDataService;
            _logger = logger;
        }

        public IActionResult Index(string? selectedName, string? selectedDate)
        {
            PopulateViewData(selectedName, selectedDate);
            return View(new TimeEntry()
            {
                Date = DateTime.Today
            });
        }

        [HttpPost]
        public IActionResult SaveEntry(string EmployeeName, DateTime Date, TimeSpan InTime, TimeSpan OutTime)
        {
            var employee = csvDataService.Employees
                .FirstOrDefault(e => (e.FirstName + " " + e.LastName) == EmployeeName);

            if (employee == null)
            {
                ModelState.AddModelError("EmployeeName", "Invalid employee selected.");
                return RedirectToAction("Index");
            }

            if (csvDataService.TimeEntries.Any(te =>
                te.EmployeeId == employee.EmployeeId && te.Date.Date == Date.Date))
            {
                ModelState.Clear();
                ModelState.AddModelError("Date", "This employee already has a time entry for the selected date");
                ViewData["ShowModal"] = true;

                PopulateViewData();

                var model = new TimeEntry
                {
                    Date = Date,
                    InTime = InTime,
                    OutTime = OutTime
                };

                return View("Index", model);
            }

            if (InTime > OutTime)
            {
                ModelState.Clear();
                ModelState.AddModelError("InTime", "InTime must be before OutTime!");
                ViewData["ShowModal"] = true;

                PopulateViewData();

                var model = new TimeEntry
                {
                    Date = Date,
                    InTime = InTime,
                    OutTime = OutTime
                };

                return View("Index", model);
            }

            var newEntry = new TimeEntry
            {
                EmployeeId = employee.EmployeeId,
                Date = Date,
                InTime = InTime,
                OutTime = OutTime
            };

            var success = csvDataService.SaveEntry(newEntry);

            return RedirectToAction("Index", new TimeEntry());
        }

        private void PopulateViewData(string? selectedName = null, string? selectedDate = null)
        {
            var timeEntryDetails = new List<(string name, TimeEntry timeEntry)>();
            foreach (var timeEntry in csvDataService.TimeEntries)
            {
                var employee = csvDataService.Employees
                    .FirstOrDefault(x => x.EmployeeId == timeEntry.EmployeeId);

                if (employee != null)
                    timeEntryDetails.Add((employee.FirstName + " " + employee.LastName, timeEntry));
            }

            ViewData["EmployeeOptions"] = csvDataService.Employees
                .Select(x => new SelectListItem
                {
                    Text = x.FirstName + " " + x.LastName,
                    Value = x.FirstName + " " + x.LastName,
                    Selected = (x.FirstName + " " + x.LastName == selectedName)
                }).OrderBy(x => x.Text).ToList();

            ViewData["DateOptions"] = csvDataService.TimeEntries
                .DistinctBy(x => x.Date)
                .OrderBy(x => x.Date)
                .Select(x => new SelectListItem
                {
                    Text = $"{x.Date:d}",
                    Value = $"{x.Date:d}",
                    Selected = ($"{x.Date:d}" == selectedDate)
                }).ToList();

            var filteredList = timeEntryDetails.OrderBy(x => x.name)
                .ThenBy(x => x.timeEntry.Date).ToList();

            if (!string.IsNullOrEmpty(selectedName))
                filteredList = filteredList.Where(x => x.name == selectedName).ToList();

            if (!string.IsNullOrEmpty(selectedDate))
                filteredList = filteredList.Where(x => $"{x.timeEntry.Date:d}" == selectedDate).ToList();

            ViewData["TimeEntryDetails"] = filteredList;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}