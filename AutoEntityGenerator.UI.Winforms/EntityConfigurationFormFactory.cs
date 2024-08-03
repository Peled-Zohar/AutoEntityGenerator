using AutoEntityGenerator.Common.CodeInfo;

namespace AutoEntityGenerator.UI
{
    public interface IEntityConfigurationFormFactory
    {
        IEntityConfigurationForm Create(Entity entity);
    }

    internal class EntityConfigurationFormFactory : IEntityConfigurationFormFactory
    {
        public IEntityConfigurationForm Create(Entity entity) => new EntityConfigurationForm(entity);
    }
}