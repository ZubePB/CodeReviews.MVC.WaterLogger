using ExerciceLogger.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;

namespace ExerciceLogger.Pages;

public class CreateModel : PageModel
{
    private readonly IConfiguration _configuration;

    [BindProperty]
    public Exercice? Exercice { get; set; }
    public List<string> ExerciceTypes { get; set; }

    public CreateModel(IConfiguration configuration) => _configuration = configuration;

    public void OnGet()
    {
        Exercice = new();
        ExerciceTypes = GetExerciceTypes();
    }

    private List<string> GetExerciceTypes()
    {
        List<string> types = [];
        using(SqliteConnection connection = new(_configuration.GetConnectionString("ConnectionString")))
        {
            connection.Open();
            SqliteCommand cmd = connection.CreateCommand();
            cmd.CommandText = "SELECT * FROM Exercices GROUP BY Type";
            SqliteDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                types.Add(reader.GetString(1));
            }
            reader.Close();
            connection.Close();
            return types;
        }
    }

    public IActionResult OnPost()
    {
        if (ModelState.IsValid)
        {
            using(SqliteConnection connection = new(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();
                SqliteCommand cmd = connection.CreateCommand();
                cmd.CommandText = $"INSERT INTO Exercices (Type,Date,Reps) VALUES('{Exercice!.Type}','{Exercice!.Date.ToString("dd-MM-yyyy")}',{Exercice.Reps})";
                cmd.ExecuteNonQuery();
                connection.Close();
            }
            return RedirectToPage("./Index");
        }
        return Page();
    }
}
