using ExerciceLogger.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using System.Globalization;

namespace ExerciceLogger.Pages;

public class ReportsModel : PageModel
{
    private readonly IConfiguration _configuration;
    public Report Reports { get; set; }
    public ReportsModel(IConfiguration configuration) => _configuration = configuration;

    public IActionResult OnGet()
    {
        try
        {
            Reports = GetReports();
            ViewData["Title"] = "Reports";
            return Page();
        }
        catch { return RedirectToPage("/Error"); }
    }

    private Report GetReports()
    {
        Report reports = new();
        List<Exercice>? allExercices = GetAllExercices();
        if (allExercices != null)
        {
            reports.ExerciceTypes = allExercices?.DistinctBy(x => x.Type).ToList();
            foreach(Exercice ex in reports.ExerciceTypes!)
            {
                List<Exercice> typeEx = allExercices!.Where(x => x.Type == ex.Type).ToList();
                ex.Reps = typeEx!.Sum(x => x.Reps);
                reports.FirstDates!.Add(typeEx.Min(x => x.Date).Date);
                reports.LastDates!.Add(typeEx.Max(x =>   x.Date).Date);
            }
        }
        return reports;
    }

    private List<Exercice>? GetAllExercices()
    {
        List<Exercice> allExercices = [];
        using (SqliteConnection connection = new(_configuration.GetConnectionString("ConnectionString")))
        {
            connection.Open();
            SqliteCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Exercices";
            SqliteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                allExercices.Add(new Exercice { Id = reader.GetInt32(0), Type = reader.GetString(1), Date = DateTime.Parse(reader.GetString(2), CultureInfo.CurrentUICulture), Reps = reader.GetInt32(3) });
            }
            reader.Close();
            connection.Close();
            return allExercices;
        }
    }
}
