using AutoEntityGenerator.Common.CodeInfo;

namespace AutoEntityGenerator.UI
{
    public class EntityConfigurationFormFactory
    {
        public IEntityConfigurationForm Create(Entity entity) => new EntityConfigurationForm(entity);
    }
}