using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;

[GenerateAuthoringComponent]
public struct GameOrientation : IComponentData
{

  [GhostField(Quantization=1000)]
  public float2 Value;

}
