
using Unity.Entities;
using Unity.NetCode;

[GenerateAuthoringComponent]
public struct AssociatedEntity : IComponentData {
  [GhostField]
  public Entity Value;
}
