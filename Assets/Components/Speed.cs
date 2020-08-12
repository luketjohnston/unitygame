using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;

[GenerateAuthoringComponent]
public struct Speed : IComponentData
{
  [GhostDefaultField(1000)]
  public float Value;

}
