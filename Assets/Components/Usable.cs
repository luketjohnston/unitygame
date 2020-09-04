using Unity.Entities;
using Unity.NetCode;

[GenerateAuthoringComponent]
public struct Usable : IComponentData {
  [GhostField]
  public bool inuse;
  [GhostField]
  public bool canuse;
}
