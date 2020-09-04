using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;

[GenerateAuthoringComponent]
public struct Health : IComponentData
{
  [GhostField(Quantization=1000)]
  public float Value;
  [GhostField(Quantization=1000)]
  public float regen;
  [GhostField(Quantization=1000)]
  public float max;

}
