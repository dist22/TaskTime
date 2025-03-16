using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManager.Models;

public class TaskTime
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int TaskId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public bool isComplete { get; set; } = false;
    public DateTime CreateAt { get; set; } = DateTime.Now;

    public List<User> Users { get; set; } = new List<User>();
    public Category? Category { get; set; }
    public int CategoryId { get; set; }
}