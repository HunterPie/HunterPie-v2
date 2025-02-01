using System.Threading.Tasks;

namespace HunterPie.Domain.Interfaces;

public interface IInitializer
{
    public Task Init();
}