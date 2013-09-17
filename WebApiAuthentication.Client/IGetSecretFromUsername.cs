namespace WebApiAuthentication.Client
{
    public interface IGetSecretFromUsername
    {
        string Secret(string username);
    }
}
