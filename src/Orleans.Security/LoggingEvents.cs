using Microsoft.Extensions.Logging;

namespace Orleans.Security
{
    public static class LoggingEvents
    {
        public static readonly EventId AccessTokenValidationFailed =
            new EventId(1001, "Access Token validation failed.");

        public static readonly EventId OutgoingGrainCallAuthorizationPassed =
            new EventId(1002, "Outgoing Grain Call Authorization Passed.");
    }
}
