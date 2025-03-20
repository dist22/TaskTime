namespace TaskManager.Dtos;

public class ChangePasswordDto
{
    public string Password { get; set; } = string.Empty;
    public string PasswordConfirm { get; set; } = string.Empty;
}