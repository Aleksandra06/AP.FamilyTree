using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace AP.FamilyTree.Web.Data.SharedService
{
    public class LoggerProvider : ILoggerProvider
    {
        private IPushNotificationsQueue _pr { get; }
        private readonly IHttpContextAccessor _httpContextAccessor;
        //private AuthenticationStateProvider _state;
        public LoggerProvider(IPushNotificationsQueue pr, IHttpContextAccessor httpContextAccessor)//, AuthenticationStateProvider state)
        {
            _httpContextAccessor = httpContextAccessor;
            _pr = pr;
            //_state = state;
        }
        public ILogger CreateLogger(string categoryName) => new Logger(_pr, _httpContextAccessor);

        public void Dispose()
        {
        }
    }
}
