using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ExerciceLogger.Models;

public class Report
{
    public List<Exercice>? ExerciceTypes { get; set; } = [];

    [Display(Name = "First Date"),DisplayFormat(DataFormatString = "{0:dd-MM-yy}", ApplyFormatInEditMode = true)]
    public List<DateTime>? FirstDates { get; set; } = [];

    [DisplayName("Last Date"), DisplayFormat(DataFormatString = "{0:dd-MM-yy}", ApplyFormatInEditMode = true)]
    public List<DateTime>? LastDates { get; set; } = [];
}