namespace HunterPie.Integrations.Datasources.MonsterHunterWorld.Definitions;

public record MHWHarvestBoxData(
    MHWItemStructure[] Items,
    MHWFertilizerStructure[] Fertilizers
);