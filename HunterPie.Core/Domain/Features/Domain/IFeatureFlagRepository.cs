namespace HunterPie.Core.Domain.Features.Domain;

public interface IFeatureFlagRepository
{
    public IFeature GetFeature(string feature);
    public bool IsEnabled(string feature);
}