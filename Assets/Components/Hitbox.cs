using Unity.NetCode;
using Unity.Entities;

[GenerateAuthoringComponent]
public struct Hitbox : IComponentData {
  [GhostField]
  public Entity hitToProcess;
}
