using System;

namespace Heimdall.Server.Windsor
{
    public class CannotResolveSecretFromUsernamesException : Exception
    {
        public CannotResolveSecretFromUsernamesException(string message) : base(message)
        {
        }
    }
}