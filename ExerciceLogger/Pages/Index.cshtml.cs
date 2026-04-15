using ExerciceLogger.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Globalization;

namespace ExerciceLogger.Pages;

public class IndexModel : PageModel
{
    private readonly IConfiguration _configuration;

    public List<Exercice>? AllExercices { get; set; }

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
        if (AllExercices != null) AllExercices = AllExercices.OrderBy(x => x.Type).ThenBy(x => x.Date).ThenBy(x => x.Reps).ToList();
        Order = "Type";
    }

    public void OnGetTypeDesc()
    {
        AllExercices = GetAllExercices();
        Asc = false;
        if (AllExercices != null) AllExercices = AllExercices.OrderByDescending(x => x.Type).ThenByDescending(x => x.Date).ThenByDescending(x => x.Reps).ToList();
        Order = "Type";
    }

    public void OnGetDate()
    {
        AllExercices = GetAllExercices();
        Asc = true;
        if (AllExercices != null) AllExercices = AllExercices.OrderBy(x => x.Date).ThenBy(x => x.Type).ThenBy(x => x.Reps).ToList();
        Order = "Date";
    }

    public void OnGetDateDesc()
    {
        AllExercices = GetAllExercices();
        if (AllExercices != null) AllExercices = AllExercices.OrderByDescending(x => x.Date).ThenByDescending(x => x.Type).ThenByDescending(x => x.Reps).ToList();
        Asc = false;
        Order = "Date";
    }

    public void OnGetReps()
    {
        AllExercices = GetAllExercices();
        Asc = true;
        if (AllExercices != null) if (AllExercices != null) AllExercices = AllExercices.OrderBy(x => x.Reps).ThenBy(x => x.Date).ThenBy(x => x.Type).ToList();
        Order = "Reps";
    }

    public void OnGetRepsDesc()
    {
        AllExercices = GetAllExercices();
        if (AllExercices != null) if (AllExercices != null) AllExercices = AllExercices.OrderByDescending(x => x.Reps).ThenByDescending(x => x.Date).ThenByDescending(x => x.Type).ToList();
        Asc = false;
        Order = "Reps";
    }


    private List<Exercice>? GetAllExercices()
    {
        List<Exercice> allExercices = [];
        try
        {
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
        catch { return null; }
    }
}
