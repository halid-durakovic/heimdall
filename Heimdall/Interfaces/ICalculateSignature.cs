namespace Heimdall.Interfaces
{
    public interface ICalculateSignature
    {
        string Calculate(string secret, string messageRepresentation);
    }
}