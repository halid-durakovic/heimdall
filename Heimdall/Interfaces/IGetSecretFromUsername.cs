namespace Heimdall.Interfaces
{
    public interface IGetSecretFromUsername
    {
        string Secret(string username);
    }
}