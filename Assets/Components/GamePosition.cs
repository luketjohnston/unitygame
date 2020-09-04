using Unity.Entities;
using Unity.NetCode;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct GamePosition : IComponentData
{
  [GhostField(Quantization=1000)]
  public float2 Value;

}
