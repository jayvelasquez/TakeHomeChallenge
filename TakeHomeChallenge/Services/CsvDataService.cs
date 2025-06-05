using System.Text;
using TakeHomeChallenge.Models;

namespace TakeHomeChallenge.Services
{
    public class CsvDataService
    {
        private readonly ILogger<CsvDataService> logger;
        private readonly string employeeFilePath = "AppData/Employees.csv";
        private readonly string timeEntriesFilePath = "AppData/TimeEntries.csv";
        private readonly List<Employee> employees = new();
        private readonly List<TimeEntry> timeEntries = new();

        public IReadOnlyList<Employee> Employees => employees;
        public IReadOnlyList<TimeEntry> TimeEntries => timeEntries;

        public CsvDataService(ILogger<CsvDataService> logger)
        {
            this.logger = logger;
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                employees.Clear();
                timeEntries.Clear();

                if (File.Exists(employeeFilePath))
                {
                    var rows = File.ReadAllLines(employeeFilePath).Skip(1);
                    foreach (var row in rows)
                    {
                        var splits = row.Split(',');
                        employees.Add(new Employee
                        {
                            EmployeeId = int.Parse(splits[0]),
                            FirstName = splits[1],
                            LastName = splits[2],
                        });
                    }
                }

                if (File.Exists(timeEntriesFilePath))
                {
                    var rows = File.ReadAllLines(timeEntriesFilePath).Skip(1);
                    foreach (var row in rows)
                    {
                        var splits = row.Split(',');
                        timeEntries.Add(new TimeEntry
                        {
                            EntryId = int.Parse(splits[0]),
                            EmployeeId = int.Parse(splits[1]),
                            Date = DateTime.Parse(splits[2]),
                            InTime = TimeSpan.Parse(splits[3]),
                            OutTime = TimeSpan.Parse(splits[4]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"Failed to load data: {ex.Message}");
            }
        }

        public bool SaveEntry(TimeEntry timeEntry)
        {
            try
            {
                var nextId = timeEntries.LastOrDefault()?.EntryId;
                if (nextId == null && timeEntries.Count > 0)
                {
                    logger.LogError($"Failed to log time entry for employee " +
                        $"{timeEntry.EmployeeId} for day {timeEntry.Date:d}:" +
                        $"Could not find last index.");

                    return false;
                }
                else if(timeEntries.Count == 0)
                {
                    nextId = 0;
                }

                var line = $"{nextId + 1},{timeEntry.EmployeeId}," +
                    $"{timeEntry.Date:yyyy-MM-dd},{timeEntry.InTime:hh\\:mm}," +
                    $"{timeEntry.OutTime:hh\\:mm}";

                File.AppendAllText(timeEntriesFilePath, "\n" + line, Encoding.UTF8);

                LoadData();
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError($"Failed to log time entry for employee " +
                    $"{timeEntry.EmployeeId} for day {timeEntry.Date:d}:" +
                    $"{ex.Message}");

                return false;
            }
        }
    }
}