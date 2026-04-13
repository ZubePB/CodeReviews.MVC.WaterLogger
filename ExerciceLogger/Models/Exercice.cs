using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ExerciceLogger.Models;

public class Exercice
{
    [Key]
    public int Id { get; set; }

    [Required,MinLength(3),MaxLength(20),Display(Name = "Exercice")]
    public string? Type { get; set; }

    [Required, DisplayFormat(DataFormatString = "{0:dd-MM-yy}", ApplyFormatInEditMode = true)]
    public DateTime Date { get; set; } = DateTime.Now;

    [Required,Range(0, int.MaxValue),DisplayName("Repetitions")]
    public int Reps { get; set; }
}
