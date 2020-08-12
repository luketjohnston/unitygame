using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;

[GenerateAuthoringComponent]
public struct DestinationComponent : IComponentData
{
  [GhostDefaultField(1000)]
  public float2 Value;
  [GhostDefaultField]
  public bool Valid;

}
