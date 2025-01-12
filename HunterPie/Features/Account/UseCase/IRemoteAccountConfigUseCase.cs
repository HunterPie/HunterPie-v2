using System.Threading.Tasks;

namespace HunterPie.Features.Account.UseCase;

internal interface IRemoteAccountConfigUseCase
{
    Task UploadAsync();
    Task DownloadAsync();
}