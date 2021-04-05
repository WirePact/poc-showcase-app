using System.Linq;
using System.Threading.Tasks;
using AspNetCore.Authentication.Basic;

namespace Legacy_Service
{
    public class BasicAuthenticator : IBasicUserValidationService
    {
        private readonly Configuration _configuration;

        public BasicAuthenticator(Configuration configuration)
        {
            _configuration = configuration;
        }

        public Task<bool> IsValidAsync(string username, string password)
            => Task.FromResult(_configuration.Credentials.Any(c => c.User == username && c.Pass == password));
    }
}
