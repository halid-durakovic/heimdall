namespace WebApiAuthentication.Client
{
    public interface IGetSecretFromKey
    {
        string Secret(string key);
    }
}