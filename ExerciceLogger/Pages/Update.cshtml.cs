using ExerciceLogger.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using System.Globalization;

namespace ExerciceLogger.Pages
{
    public class UpdateModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public List<string>? ExerciceTypes { get; set; }

        [BindProperty]
        public Exercice? Exercice { get; set; }

        public UpdateModel(IConfiguration configuration) => _configuration = configuration;

        public void OnGet(int id)
        {
            try
            { 
                ExerciceTypes = GetExerciceTypes();
                Exercice = GetExercice(id);
            }
            catch
            {
                ExerciceTypes = null;
                Exercice = null;
            }

        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (SqliteConnection connection = new(_configuration.GetConnectionString("ConnectionString")))
                    {
                        connection.Open();
                        SqliteCommand cmd = connection.CreateCommand();
                        cmd.CommandText = $"UPDATE Exercices SET Type = '{Exercice.Type}',Date = '{Exercice.Date.ToString("dd-MM-yyyy")}',Reps = {Exercice.Reps} WHERE Id = {Exercice.Id}";
                        cmd.ExecuteNonQuery();
                        connection.Close();
                    }
                    return RedirectToPage("./Index");
                }
                catch { return RedirectToPage("/Error"); }
            }
            return Page();
        }

        private Exercice GetExercice(int id)
        {
            using (SqliteConnection connection = new(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();
                SqliteCommand cmd = connection.CreateCommand();
                cmd.CommandText = $"SELECT * FROM Exercices WHERE Id = {id};";
                SqliteDataReader reader = cmd.ExecuteReader();
                reader.Read();
                Exercice ex = new Exercice
                {
                    Id = id,
                    Type = reader.GetString(1),
                    Date = DateTime.Parse(reader.GetString(2), CultureInfo.CurrentUICulture),
                    Reps = reader.GetInt32(3),
                };
                reader.Close();
                connection.Close();
                return ex;
            }
        }

        private List<string> GetExerciceTypes()
        {
            List<string> types = [];
            using (SqliteConnection connection = new(_configuration.GetConnectionString("ConnectionString")))
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
    }
}
