using Unity.Entities;
using Unity.NetCode;

[GenerateAuthoringComponent]
public struct Usable : IComponentData {
  [GhostDefaultField]
  public bool inuse;
  [GhostDefaultField]
  public bool canuse;
}
