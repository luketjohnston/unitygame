using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;

[GenerateAuthoringComponent]
public struct AngleInput : IComponentData
{

  [GhostField(Quantization=1000)]
  public float Value;

}
