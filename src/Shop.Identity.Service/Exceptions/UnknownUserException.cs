using System;

namespace Shop.Identity.Service.Exceptions
{
    [Serializable]
    public class UnknownUserException : Exception
    {
        public Guid UserId { get; }

        public UnknownUserException(Guid userId)
            : base($"Unknown user '{userId}'")
        {
            UserId = userId;
        }
    }
}
