namespace TaskManager.Interfaces;

public interface IPasswordHasher
{
    public string Genarate(string paasword);

    public bool Verify(string password, string passwordHash);
}