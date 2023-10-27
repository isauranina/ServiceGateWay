namespace Nur.Gateway.Web.Handlers
{
    public class LogDelegatingHandler : DelegatingHandler
    {
        private readonly ILogger<LogDelegatingHandler> _logger;

        public LogDelegatingHandler(ILogger<LogDelegatingHandler> logger)
        {
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Request: {request}");
            var response = await base.SendAsync(request, cancellationToken);
            _logger.LogInformation($"Response: {response}");
            return response;
        }
    }
}
