using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;

[GenerateAuthoringComponent]
public struct Health : IComponentData
{
  [GhostDefaultField(1000)]
  public float Value;
  [GhostDefaultField(1000)]
  public float regen;
  [GhostDefaultField(1000)]
  public float max;

}
