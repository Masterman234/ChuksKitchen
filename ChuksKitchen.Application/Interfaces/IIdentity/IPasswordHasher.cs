namespace ChuksKitchen.Application.Interfaces.IIdentity;

public interface IPasswordHasher
{
        string Hash(string password);
        bool Verify(string password, string hash);
}
