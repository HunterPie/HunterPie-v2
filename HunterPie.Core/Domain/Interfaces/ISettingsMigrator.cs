using System;

namespace HunterPie.Core.Domain.Interfaces;

public interface ISettingsMigrator
{
    public bool CanMigrate(IVersionedConfig oldSettings);
    public Type GetRequiredType();
    public IVersionedConfig Migrate(IVersionedConfig oldSettings);
}