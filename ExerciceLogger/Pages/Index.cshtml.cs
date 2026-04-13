using ExerciceLogger.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using System.Globalization;

namespace ExerciceLogger.Pages;

public class IndexModel : PageModel
{
    private readonly IConfiguration _configuration;

    public List<Exercice> AllExercices { get; set; }

    [BindProperty]
    public string? Order { get; set; }

    public bool Asc { get; set; } = true;

    public string BtnImgSrc { get; set; } = "~/img/arrow-down-3101_16.png";

    public IndexModel(IConfiguration configuration) => _configuration = configuration;

    public void OnGet()
    {
        AllExercices = GetAllExercices();
    }

    public void OnGetType()
    {
        AllExercices = GetAllExercices();
        Asc = true;
        AllExercices = AllExercices.OrderBy(x => x.Type).ToList();
        Order = "Type";
    }

    public void OnGetTypeDesc()
    {
        AllExercices = GetAllExercices();
        Asc = false;
        AllExercices = AllExercices.OrderByDescending(x => x.Type).ToList();
        Order = "Type";
    }

    public void OnGetDate()
    {
        AllExercices = GetAllExercices();
        Asc = true;
        AllExercices = AllExercices.OrderBy(x => x.Date).ToList();
        Order = "Date";

    }

    public void OnGetDateDesc()
    {
        AllExercices = GetAllExercices();
        AllExercices = AllExercices.OrderByDescending(x => x.Date).ToList(); // : AllExercices.OrderBy(x => x.Date).ToList();
        Asc = false;
        Order = "Date";
    }

    public void OnGetReps()
    {
        AllExercices = GetAllExercices();
        Asc = true;
        AllExercices = AllExercices.OrderBy(x => x.Reps).ToList();
        Order = "Reps";
    }

    public void OnGetRepsDesc()
    {
        AllExercices = GetAllExercices();
        AllExercices = Asc ? AllExercices.OrderByDescending(x => x.Reps).ToList() : AllExercices.OrderBy(x => x.Reps).ToList();
        Asc = false;
        Order = "Reps";
    }


    private List<Exercice> GetAllExercices()
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
