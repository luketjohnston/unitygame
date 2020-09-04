using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;

[GenerateAuthoringComponent]
public struct OrientationInput : IComponentData
{

  [GhostField(Quantization=1000)]
  public float2 Value;

}
