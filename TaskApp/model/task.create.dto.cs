using System.ComponentModel.DataAnnotations;

namespace TaskModels;

public class TaskCreate
{
    public int id { get; set; }
    [Required, StringLength(100)]
    public string title { get; set; }
    public string? description { get; set; }
}