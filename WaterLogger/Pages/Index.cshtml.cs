using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using System.Globalization;
using WaterLogger.Models;

namespace WaterLogger.Pages;

public class IndexModel : PageModel
{
    public IConfiguration _configuration { get; set; }
    public List<DrinkingWater> Records { get; set; }

    public IndexModel(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void OnGet()
    {
        Records = GetAllRecords();
        ViewData["Total"] = Records.Sum(x => x.Quantity);
    }

    private List<DrinkingWater> GetAllRecords()
    {
        List<DrinkingWater> AllRecords = [];
        using(SqliteConnection connection = new(_configuration.GetConnectionString("ConnectionString")))
        {
            connection.Open();
            SqliteCommand sqliteCmd = connection.CreateCommand();
            sqliteCmd.CommandText = "SELECT * FROM drinking_water";
            SqliteDataReader reader = sqliteCmd.ExecuteReader();

            while (reader.Read())
            {
                AllRecords.Add(
                    new DrinkingWater
                    {
                        Id = reader.GetInt32(0),
                        Date = DateTime.Parse(reader.GetString(1),CultureInfo.CurrentUICulture.DateTimeFormat),
                        Quantity = reader.GetInt32(2),
                    });
            }
            return AllRecords;
        }
    }
}
