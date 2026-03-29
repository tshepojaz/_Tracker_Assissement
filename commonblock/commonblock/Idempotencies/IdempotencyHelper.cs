using Microsoft.AspNetCore.Http;

namespace commonblock.Idempotencies;

public static class IdempotencyHelper
{
    public const string IdempotencyKeyHeaderName = "Idempotency-Key";

    // This is our custom TryGetIdempotencyKey implementation for HTTP requests.
    public static bool TryGetIdempotencyKey(HttpRequest request, out Guid idempotencyKey)
    {
        idempotencyKey = Guid.Empty;

        if (request.Headers.TryGetValue(IdempotencyKeyHeaderName, out var keyValues) &&
            !string.IsNullOrEmpty(keyValues.FirstOrDefault()))
        {
            idempotencyKey = Guid.Parse(keyValues.First()!);
            return true;
        }
        return false;
    }
}
