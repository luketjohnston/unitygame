using Unity.Entities;
using Unity.NetCode;

[GenerateAuthoringComponent]
public struct Cooldown : IComponentData {
  [GhostDefaultField(1000)]
  public float timer;
  [GhostDefaultField(1000)]
  public float duration;
}
