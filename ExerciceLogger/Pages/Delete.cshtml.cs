using ExerciceLogger.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using System.Globalization;

namespace ExerciceLogger.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly IConfiguration _configuration;

        [BindProperty]
        public Exercice Exercice { get; set; }

        public DeleteModel(IConfiguration configuration) => _configuration = configuration;

        public void OnGet(int id)
        {
            Exercice = GetExercice(id);
        }

        private Exercice GetExercice(int id)
        {
            using(SqliteConnection connection = new(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();
                SqliteCommand cmd = connection.CreateCommand();
                cmd.CommandText = $"SELECT * FROM Exercices WHERE Id = {id};";
                SqliteDataReader reader = cmd.ExecuteReader();
                reader.Read();
                Exercice ex = new Exercice { Id = id, Type = reader.GetString(1), Date = DateTime.Parse(reader.GetString(2),CultureInfo.CurrentUICulture), Reps = reader.GetInt32(3), };
                reader.Close();
                connection.Close();
                return ex;
            }
        }

        public IActionResult OnPost()
        {
            using (SqliteConnection connection = new(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();
                SqliteCommand cmd = connection.CreateCommand();
                cmd.CommandText = $"DELETE FROM Exercices WHERE Id = {Exercice.Id}";
                cmd.ExecuteNonQuery();
                connection.Close();
                return RedirectToPage("./Index");
            }
        }
    }
}
