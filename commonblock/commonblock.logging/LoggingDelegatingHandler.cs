using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Sockets;

namespace commonblock.logging;

public class LoggingDelegatingHandler : DelegatingHandler
{
        private readonly ILogger<LoggingDelegatingHandler> logger;

        public LoggingDelegatingHandler(ILogger<LoggingDelegatingHandler> logger)
        {
            this.logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                logger.LogInformation("Sending request to {Url}", request.RequestUri);

                var response = await base.SendAsync(request, cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    if (response.RequestMessage?.RequestUri != null)
                    {
                        logger.LogInformation("Received a success response from {Url}", response.RequestMessage.RequestUri);
                    }
                    else
                    {
                        logger.LogInformation("Received a success response, but the RequestUri is null.");
                    }
                }
                else
                {
                    logger.LogWarning("Received a non-success status code {StatusCode} from {Url}",
                        (int)response.StatusCode, response.RequestMessage?.RequestUri?.ToString() ?? "Unknown URL");
                }

                return response;
            }
            catch (HttpRequestException ex)
                when (ex.InnerException is SocketException se && se.SocketErrorCode == SocketError.ConnectionRefused)
            {
                var hostWithPort = request.RequestUri != null && request.RequestUri.IsDefaultPort
                    ? request.RequestUri.DnsSafeHost
                    : request.RequestUri != null 
                        ? $"{request.RequestUri.DnsSafeHost}:{request.RequestUri.Port}"
                        : "Unknown Host";

                logger.LogCritical(ex, "Unable to connect to {Host}. Please check the " +
                                        "configuration to ensure the correct URL for the service " +
                                        "has been configured.", hostWithPort);
            }

            return new HttpResponseMessage(HttpStatusCode.BadGateway)
            {
                RequestMessage = request
            };
        }
    }
