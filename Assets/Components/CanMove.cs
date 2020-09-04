using Unity.Entities;
using Unity.Mathematics;
using Unity.NetCode;

[GenerateAuthoringComponent]
public struct CanMove : IComponentData
{
  [GhostField]
  public bool Value;

}
