using HunterPie.Core.Domain.Features.Domain;

namespace HunterPie.Core.Domain.Features.Repository;

public interface IFeatureFlagRepository
{
    public IFeature? GetFeature(string feature);
    public bool IsEnabled(string feature);
}