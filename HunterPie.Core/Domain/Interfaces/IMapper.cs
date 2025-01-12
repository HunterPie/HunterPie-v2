namespace HunterPie.Core.Domain.Interfaces;

public interface IMapper<in TIn, out TOut>
{
    public TOut Map(TIn data);
}