using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;

[GenerateAuthoringComponent]
public struct CanMove : IComponentData
{
  [GhostDefaultField]
  public bool Value;

}
