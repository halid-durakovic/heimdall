﻿using System;
using System.Net.Http;

namespace Heimdall
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
            if (string.IsNullOrEmpty(secret))
                throw new ArgumentNullException("secret");

            var messageRepresentation = buildMessageRepresentation.Build(request);
            var hash = calculateSignature.Calculate(secret, messageRepresentation);
            return hash;
        }
    }
}
