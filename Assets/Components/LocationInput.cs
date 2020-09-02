
using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;

[GenerateAuthoringComponent]
public struct LocationInput : IComponentData
{

  [GhostDefaultField(1000)]
  public float2 Value;

}
