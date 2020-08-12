using Unity.Entities;
using Unity.NetCode;

[GenerateAuthoringComponent]
public struct Usable : IComponentData {
  public bool inuse;
  [GhostDefaultField]
  public bool canuse;
}
