namespace TaskManager.Models;

public class JwtOptions
{
    public string TokenKey { get; set; } = string.Empty;

    public int ExpitesHours { get; set; }
}