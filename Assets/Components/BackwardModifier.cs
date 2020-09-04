using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;

[GenerateAuthoringComponent]
public struct BackwardModifier : IComponentData
{
  [GhostField(Quantization=1000)]
  public float Value;

}
