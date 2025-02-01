using HunterPie.Core.Client;
using HunterPie.Core.Client.Configuration.Enums;
using HunterPie.Core.Remote;
using System.IO;
using System.Threading.Tasks;

namespace HunterPie.UI.Architecture.Adapter;
public class MonsterIconAdapter
{
    public static async Task<string> UriFrom(GameType game, int monsterId)
    {
        string monsterEm = $"{game}_{monsterId:00}";
        string localUri = ClientInfo.GetPathFor($"Assets/Monsters/Icons/{monsterEm}.png");

        if (!File.Exists(localUri))
            localUri = await CDN.GetMonsterIconUrl(monsterEm);

        return localUri;
    }
}