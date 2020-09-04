using Unity.Entities;
using Unity.NetCode;

[GenerateAuthoringComponent]
public struct AgentComponent : IComponentData
{
    [GhostField]
    public int PlayerId;
}
