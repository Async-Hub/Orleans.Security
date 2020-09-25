using Microsoft.Extensions.Logging;

namespace Orleans.Security
{
    public static class LoggingEvents
    {
        public static readonly EventId AccessTokenValidationFailed =
            new EventId(938003, "Access Token validation failed.");

        public static readonly EventId OutgoingGrainCallAuthorizationPassed =
            new EventId(938001, "Outgoing Grain Call Authorization Passed.");

        public static readonly EventId AccessTokenVerified =
            new EventId(938004, "Access Token Verified.");
        
        public static readonly EventId IncomingGrainCallAuthorizationPassed =
            new EventId(938002, "Incoming Grain Call Authorization Passed.");
    }
}
