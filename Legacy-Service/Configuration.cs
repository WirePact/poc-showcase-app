using System.Collections.Generic;

namespace Legacy_Service
{
    public class Configuration
    {
        public List<Credential> Credentials { get; init; } = new();
    }

    public record Credential
    {
        public string User { get; init; } = string.Empty;

        public string Pass { get; init; } = string.Empty;
    }
}
