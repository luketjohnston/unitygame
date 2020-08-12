using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;

[GenerateAuthoringComponent]
public struct BackwardModifier : IComponentData
{
  [GhostDefaultField(1000)]
  public float Value;

}
