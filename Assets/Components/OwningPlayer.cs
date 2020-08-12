using Unity.Entities;
using Unity.NetCode;

[GenerateAuthoringComponent]
public struct OwningPlayer : IComponentData {
  [GhostDefaultField]
  public Entity Value;
  [GhostDefaultField]
  public int PlayerId;
}
