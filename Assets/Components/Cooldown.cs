using Unity.Entities;
using Unity.NetCode;

[GenerateAuthoringComponent]
public struct Cooldown : IComponentData {
  [GhostField(Quantization=1000)]
  public float timer;
  [GhostField(Quantization=1000)]
  public float duration;
}
