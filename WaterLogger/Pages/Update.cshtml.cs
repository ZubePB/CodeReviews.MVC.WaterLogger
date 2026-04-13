using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using System.Globalization;
using WaterLogger.Models;

namespace WaterLogger.Pages;

public class UpdateModel : PageModel
{
    public IConfiguration _configuration { get; set; }
    [BindProperty]
    public DrinkingWater DrinkingWater { get; set; }
    public UpdateModel(IConfiguration configuration) => _configuration = configuration;
    public void OnGet(int id)
    {
        DrinkingWater = GetById(id);
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
    public IActionResult OnPost()
    {
        if (ModelState.IsValid)
        {
            using (SqliteConnection connection = new(_configuration.GetConnectionString("ConnectionString")))
            {
                connection.Open();
                SqliteCommand sqliteCmd = connection.CreateCommand();
                sqliteCmd.CommandText = $"UPDATE drinking_water SET Date ='{DrinkingWater.Date.ToString("yyyy.MM.dd HH:mm")}', Quantity = {DrinkingWater.Quantity} WHERE Id = {DrinkingWater.Id}";
                sqliteCmd.ExecuteNonQuery();
                connection.Close();
            }
            return RedirectToPage("./Index");
        }
        return Page();
    }
}
