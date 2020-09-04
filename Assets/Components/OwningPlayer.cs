using Unity.Entities;
using Unity.NetCode;

[GenerateAuthoringComponent]
public struct OwningPlayer : IComponentData {
  [GhostField]
  public Entity Value;
  [GhostField]
  public int PlayerId;
}
