namespace TaskManager.Dtos;

public class TaskDto
{
    public int TaskId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public bool isComplete { get; set; }
    public DateTime CreateAt { get; set; }
}