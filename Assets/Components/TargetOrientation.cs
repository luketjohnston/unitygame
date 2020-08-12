using Unity.Entities;
using Unity.NetCode;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct TargetOrientation : IComponentData
{
  [GhostDefaultField(1000)]
  public float2 Value;
}
