using TaskManager.Interfaces;

namespace TaskManager.Hasher;

public class PasswordHasher : IPasswordHasher
{
    public string Genarate(string paasword) =>
        BCrypt.Net.BCrypt.EnhancedHashPassword(paasword);

    public bool Verify(string password, string passwordHash) =>
        BCrypt.Net.BCrypt.EnhancedVerify(password, passwordHash);

}