using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;

[GenerateAuthoringComponent]
public struct BusyTimer : IComponentData
{
  [GhostDefaultField(1000)]
  public float Value;
}
