using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using System.Globalization;
using WaterLogger.Models;

namespace WaterLogger.Pages;

public class DeleteModel : PageModel
{
    public IConfiguration _configuration { get; set; }
    public DrinkingWater DrinkingWater { get; set; }

    public DeleteModel(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IActionResult OnGet(int id)
    {
        DrinkingWater = GetById(id);
        return Page();
    }

    private DrinkingWater GetById(int id)
    {
        DrinkingWater drinkingWater;
        using (SqliteConnection connection = new(_configuration.GetConnectionString("ConnectionString")))
        {
            connection.Open();
            SqliteCommand sqliteCmd = connection.CreateCommand();
            sqliteCmd.CommandText = $"SELECT * FROM drinking_water WHERE Id = {id}";
            List<DrinkingWater> tableData = [];
            SqliteDataReader reader = sqliteCmd.ExecuteReader();
            reader.Read();
            drinkingWater = new DrinkingWater
            {
                Id = reader.GetInt32(0),
                Date = DateTime.Parse(reader.GetString(1), CultureInfo.CurrentUICulture.DateTimeFormat),
                Quantity = reader.GetInt32(2),
            };
            return drinkingWater;
        }
    }

    public IActionResult OnPost(int id)
    {
        using (SqliteConnection connection = new(_configuration.GetConnectionString("ConnectionString")))
        {
            connection.Open();
            SqliteCommand sqliteCmd = connection.CreateCommand();
            sqliteCmd.CommandText = $"DELETE FROM drinking_water WHERE Id = {id}";
            sqliteCmd.ExecuteNonQuery();
            connection.Close();
        }
        return RedirectToPage("./Index");
    }
}
