using AutoEntityGenerator.Common.CodeInfo;

namespace AutoEntityGenerator.Common.Interfaces;

public interface IUserInteraction
{
    IUserInteractionResult ShowUIForm(Entity entityInfo);
}
