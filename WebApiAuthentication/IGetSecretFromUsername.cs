namespace WebApiAuthentication
{
    public interface IGetSecretFromUsername
    {
        string Secret(string username);
    }
}
