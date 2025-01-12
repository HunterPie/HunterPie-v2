using HunterPie.Core.Json;
using HunterPie.Core.Logger;
using HunterPie.Core.Networking.Http;
using System.Net;
using System.Threading.Tasks;

namespace HunterPie.Integrations.Poogie.Common.Models;

internal record PoogieResult<T>(
    T? Response,
    PoogieError? Error
)
{
    public static async Task<PoogieResult<T>> FromAsync(HttpClientResponse response)
    {
        string? rawResponse = await response.AsTextAsync();

        if (rawResponse is null)
            return new PoogieResult<T>(Response: default(T), Error: PoogieError.Default());

        var resp = default(T);
        PoogieError? error = null;
        try
        {
            error = JsonProvider.Deserializer<PoogieError>(rawResponse);
        }
        catch
        { }

        if (error is null && response.StatusCode >= HttpStatusCode.BadRequest)
            error = new PoogieError(PoogieErrorCode.UNKNOWN_ERROR, "Unmapped error");

        if (error is null || error.Code == PoogieErrorCode.NOT_ERROR)
            try
            {
                resp = JsonProvider.Deserializer<T>(rawResponse);
                error = null;
            }
            catch
            {
                Log.Error("Failed to deserialize response body to JSON");
            }

        return new PoogieResult<T>(Response: resp, Error: error);
    }
}