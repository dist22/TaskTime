using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Models;

public class Category
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int CategoriesId { get; set; }

    public string CategoriesName { get; set; } = string.Empty;

    public List<TaskTime> Tasks { get; set; } = new List<TaskTime>();

}