using HunterPie.Core.Domain.Mapper;
using HunterPie.Core.Domain.Mapper.Internal;
using HunterPie.Domain.Interfaces;
using HunterPie.Integrations.Datasources.MonsterHunterRise.Services;
using System.Threading.Tasks;

namespace HunterPie.Internal.Initializers;

internal class MapperFactoryInitializer : IInitializer
{
    public Task Init()
    {
        MapFactory.Add(new GameTypeToGameProcessMapper());
        MapFactory.Add(new XmlNodeToAbnormalityDefinitionMapper(
            new MHRAbnormalityFlagTypeParser()
        ));
        MapFactory.Add(new GameProcessToGameTypeMapper());
        MapFactory.Add(new XmlNodeToMonsterDefinitionMapper());
        MapFactory.Add(new XmlNodeToMonsterPartDefinitionMapper());
        MapFactory.Add(new XmlNodeToMonsterSizeDefinitionMapper());
        MapFactory.Add(new XmlNodeToAilmentDefinitionMapper());

        return Task.CompletedTask;
    }
}