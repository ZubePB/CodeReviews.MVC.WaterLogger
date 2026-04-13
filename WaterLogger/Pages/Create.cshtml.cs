using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.Sqlite;
using WaterLogger.Models;

namespace WaterLogger.Pages;

public class CreateModel : PageModel
{
    public IConfiguration _configuration { get; set; }


    [BindProperty]
    public DrinkingWater DrinkingWater {get;set;}
    public IActionResult OnGet()
    {
        DrinkingWater = new();
        return Page();
    }

    public CreateModel(IConfiguration configuration) => _configuration = configuration;

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        using (SqliteConnection connection = new SqliteConnection(_configuration.GetConnectionString("ConnectionString")))
        {
            connection.Open();
            var tableCmd = connection.CreateCommand();
            tableCmd.CommandText = $"INSERT INTO drinking_water(date,quantity) VALUES('{DrinkingWater.Date.ToString("yyyy.MM.dd HH:mm")}','{DrinkingWater.Quantity}')";
            tableCmd.ExecuteNonQuery();
            connection.Close();
        };

        return RedirectToPage("./Index");
    }
}
