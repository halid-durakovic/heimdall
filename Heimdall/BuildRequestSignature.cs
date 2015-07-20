using System;
using System.Net.Http;
using Heimdall.Interfaces;

namespace Heimdall
{
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
            if (string.IsNullOrEmpty(secret))
                throw new ArgumentNullException("secret");

            var messageRepresentation = buildMessageRepresentation.Build(request);

            return calculateSignature.Calculate(secret, messageRepresentation);
        }
    }
}