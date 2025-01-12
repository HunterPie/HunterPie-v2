using HunterPie.Integrations.Poogie.Common;
using HunterPie.Integrations.Poogie.Common.Models;
using HunterPie.Integrations.Poogie.Notification.Models;
using System.Threading.Tasks;

namespace HunterPie.Integrations.Poogie.Notification;
internal class PoogieNotificationConnector
{
    private readonly IPoogieClientAsync _client;

    private const string NOTIFICATION_ENDPOINT = "/v1/notifications";

    public PoogieNotificationConnector(IPoogieClientAsync client)
    {
        _client = client;
    }

    public async Task<PoogieResult<NotificationResponse[]>> FindAll() =>
        await _client.GetAsync<NotificationResponse[]>(NOTIFICATION_ENDPOINT);
}