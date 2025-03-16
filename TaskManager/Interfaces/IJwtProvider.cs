namespace TaskManager.Interfaces;

public interface IJwtProvider
{
    public string CreateToken(int userId);
}