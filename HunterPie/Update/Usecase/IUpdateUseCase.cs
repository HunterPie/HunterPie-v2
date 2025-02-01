using System.Threading.Tasks;

namespace HunterPie.Update.UseCase;

public interface IUpdateUseCase
{
    Task<bool> InvokeAsync();
}