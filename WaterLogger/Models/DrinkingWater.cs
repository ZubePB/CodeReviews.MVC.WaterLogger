using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WaterLogger.Models;

public class DrinkingWater
{
    public int Id { get; set; }

    [BindProperty,DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}",ApplyFormatInEditMode = true)]
    public DateTime Date { get; set; } = DateTime.Now;

    [BindProperty,Range(0,Int32.MaxValue,ErrorMessage = "Value for {0} must be positive")]
    public int Quantity { get; set; }
}
