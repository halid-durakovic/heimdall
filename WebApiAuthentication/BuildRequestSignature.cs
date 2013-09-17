using System.Net.Http;

namespace WebApiAuthentication
{
    public interface IBuildRequestSignature
    {
        string Build(string secret, HttpRequestMessage request);
    }

    public class BuildRequestSignature : IBuildRequestSignature
    {
        private readonly IBuildMessageRepresentation buildMessageRepresentation;
        private readonly ICalculateSignature calculateSignature;

        public BuildRequestSignature()
            : this(new BuildMessageRepresentation(), new CalculateSignature())
        { }

        public BuildRequestSignature(IBuildMessageRepresentation buildMessageRepresentation, ICalculateSignature calculateSignature)
        {
            this.buildMessageRepresentation = buildMessageRepresentation;
            this.calculateSignature = calculateSignature;
        }

        public string Build(string secret, HttpRequestMessage request)
        {
            var messageRepresentation = buildMessageRepresentation.Build(request);

            return calculateSignature.Calculate(secret, messageRepresentation);
        }
    }
}
