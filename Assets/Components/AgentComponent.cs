using Unity.Entities;
using Unity.NetCode;

[GenerateAuthoringComponent]
public struct AgentComponent : IComponentData
{
    [GhostDefaultField]
    public int PlayerId;
}
